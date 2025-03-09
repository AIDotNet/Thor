using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Thor.Abstractions.Dtos;
using Thor.BuildingBlocks.Event;
using Thor.Service.Eto;
using Thor.Service.Options;

namespace Thor.Service.Service;

public partial class UserService(
    IServiceProvider serviceProvider,
    IServiceCache cache,
    EmailService emailService,
    IEventBus<CreateUserEto> eventBus,
    IServiceCache memoryCache)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public async ValueTask<User> CreateAsync(CreateUserInput input)
    {
        // 判断账号和密码长度是否满足5位
        if (input.UserName.Length < 5 || input.Password.Length < 5) throw new Exception("用户名和密码长度不能小于5位");

        // 使用正则表达式检测账号是否由英文和数字组成
        if (!CheckUserName().IsMatch(input.UserName))
            throw new Exception("用户名只能由英文和数字组成");

        if (SettingService.GetSetting<bool>(SettingExtensions.SystemSetting.EnableEmailRegister))
        {
            // 判断邮箱是否正确使用正则表达式
            if (!CheEmail().IsMatch(input.Email))
            {
                throw new Exception("邮箱格式错误");
            }

            // 判断验证码是否正确
            var code = await memoryCache.GetAsync<string>("email-" + input.Email);
            if (code != input.Code) throw new Exception("验证码错误");
        }

        // 判断是否存在
        var exist = await DbContext.Users.AnyAsync(x => x.UserName == input.UserName || x.Email == input.Email);
        if (exist)
            throw new Exception("用户名已存在");

        if (input.Groups.Length == 0)
        {
            // 默认分组
            input.Groups = ["default"];
        }
        else if (input.Groups.All(x => x != "default"))
        {
            input.Groups = input.Groups.Append("default").ToArray();
        }

        var user = new User(Guid.NewGuid().ToString(), input.UserName, input.Email, input.Password)
        {
            Groups = input.Groups
        };

        // 初始用户额度
        var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
        user.SetResidualCredit(userQuota);

        await DbContext.Users.AddAsync(user);

        // 发送创建用户事件
        await eventBus.PublishAsync(new CreateUserEto()
        {
            User = user,
            Source = CreateUserSource.System
        });

        if (SettingService.GetSetting<bool>(SettingExtensions.SystemSetting.EnableEmailRegister))
        {
            await memoryCache.RemoveAsync("email-" + input.Email);
        }

        return user;
    }

    public async Task GetEmailCodeAsync(string email)
    {
        var code = StringHelper.GenerateRandomString(4).ToUpper();

        // 判断是否已经发送过验证码
        if (await memoryCache.ExistsAsync("email-" + email + "-send"))
        {
            throw new Exception("请勿频繁发送验证码");
        }

        await memoryCache.CreateAsync("email-" + email, code, TimeSpan.FromMinutes(5));

        // 增加缓存标识放置频繁发送
        await memoryCache.CreateAsync("email-" + email + "-send", "1", TimeSpan.FromMinutes(1));

        await emailService.SendEmailAsync(email, "注册账号验证码", $"欢迎您注册Thor（雷神托尔），您的验证码是：{code}").ConfigureAwait(false);
    }

    public async Task<User?> GetCacheAsync()
    {
        return await memoryCache.GetOrCreateAsync("userinfo:" + UserContext.CurrentUserId,
            (async () => await GetAsync(UserContext.CurrentUserId)));
    }

    public async Task<User?> GetAsync(string? id = null)
    {
        User? user;

        user = await DbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == (id ?? UserContext.CurrentUserId));

        if (user == null)
            throw new UnauthorizedAccessException();

        return user;
    }

    public async ValueTask<bool> RemoveAsync(string id)
    {
        if (UserContext.CurrentUserId == id)
            throw new Exception("不能删除自己");

        var result = await DbContext.Users.Where(x => x.Id == id && x.Role != RoleConstant.Admin)
            .ExecuteDeleteAsync();

        await RefreshUserAsync(UserContext.CurrentUserId);

        await cache.RemoveAsync("userinfo:" + UserContext.CurrentUserId);

        return result > 0;
    }

    public async ValueTask<PagingDto<User>> GetListAsync(int page, int pageSize, string? keyword)
    {
        var total = await DbContext.Users.CountAsync(x =>
            string.IsNullOrEmpty(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));

        if (total > 0)
        {
            var result = await DbContext.Users
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword))
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<User>(total, result);
        }

        return new PagingDto<User>(total, []);
    }

    /// <summary>
    ///     对于用户进行消费
    /// </summary>
    /// <param name="id"></param>
    /// <param name="consume"></param>
    /// <param name="consumeToken"></param>
    /// <param name="token"></param>
    /// <param name="channelId"></param>
    /// <param name="model">模型</param>
    /// <returns></returns>
    public async ValueTask<bool> ConsumeAsync(string id, long consume, int consumeToken, string? token,
        string channelId, string model)
    {
        using var activity =
            Activity.Current?.Source.StartActivity("用户消费扣款");

        if (ChatCoreOptions.FreeModel?.EnableFree == true)
        {
            var item = ChatCoreOptions.FreeModel.Items?.FirstOrDefault(x => x.Model.Contains(model));
            if (item != null)
            {
                // 获取当前时间到当天结束的时间
                var now = DateTime.Now;
                var end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                var remain = end - now;
                var key = "FreeModal:" + id;
                var value = await cache.GetOrCreateAsync(key, async () => await Task.FromResult(0), remain);

                // 如果没有超过限制则扣除免费次数然后返回
                if (value < item.Limit)
                {
                    await cache.IncrementAsync(key);

                    await DbContext
                        .Users
                        .Where(x => x.Id == id && x.ResidualCredit >= consume)
                        .ExecuteUpdateAsync(x =>
                            x.SetProperty(y => y.RequestCount, y => y.RequestCount + 1)
                                .SetProperty(y => y.ConsumeToken, y => y.ConsumeToken + consumeToken));

                    if (!string.IsNullOrEmpty(token))
                        await DbContext
                            .Tokens.Where(x => x.Key == token)
                            .ExecuteUpdateAsync(x =>
                                x.SetProperty(y => y.RemainQuota, y => y.RemainQuota - consume)
                                    .SetProperty(y => y.AccessedTime, DateTime.Now)
                                    .SetProperty(y => y.UsedQuota, y => y.UsedQuota + consume));

                    await DbContext
                        .Channels
                        .Where(x => x.Id == channelId)
                        .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quota, y => y.Quota + consume));

                    return true;
                }
            }
        }

        var result = await DbContext
            .Users
            .Where(x => x.Id == id && x.ResidualCredit >= consume)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.ResidualCredit, y => y.ResidualCredit - consume)
                    .SetProperty(y => y.RequestCount, y => y.RequestCount + 1)
                    .SetProperty(y => y.ConsumeToken, y => y.ConsumeToken + consumeToken));

        activity?.SetTag("消费金额", consume);
        activity?.SetTag("消费token", consumeToken);

        if (!string.IsNullOrEmpty(token))
            await DbContext
                .Tokens.Where(x => x.Key == token)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.RemainQuota, y => y.RemainQuota - consume)
                        .SetProperty(y => y.AccessedTime, DateTime.Now)
                        .SetProperty(y => y.UsedQuota, y => y.UsedQuota + consume));

        await DbContext
            .Channels
            .Where(x => x.Id == channelId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Quota, y => y.Quota + consume));

        return result > 0;
    }

    [Authorize(Roles = RoleConstant.Admin)]
    public async ValueTask UpdateAsync(UpdateUserInput input)
    {
        // 先判断是否已经存在
        if (await DbContext.Users.AnyAsync(x => x.Id != input.Id && x.Email == input.Email))
            throw new Exception("邮箱已存在");

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == input.Id);

        user.Email = input.Email;
        user.Groups = input.Groups;

        DbContext.Users.Update(user);

        await cache.RemoveAsync("userinfo:" + UserContext.CurrentUserId);

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 修改用户信息
    /// </summary>
    [Authorize]
    public async ValueTask UpdateInfoAsync(UpdateUserInfoInput input)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == UserContext.CurrentUserId);
        if (user == null)
            throw new UnauthorizedAccessException();

        // 先判断是否已经存在
        if (await DbContext.Users.AnyAsync(x => x.Id != UserContext.CurrentUserId && x.Email == input.Email))
            throw new Exception("邮箱已存在");

        if (!CheckUserName().IsMatch(input.UserName))
            throw new Exception("用户名只能由英文和数字组成");

        // 先判断是否已经存在
        if (await DbContext.Users.AnyAsync(x => x.Id != UserContext.CurrentUserId && x.UserName == input.UserName))
            throw new Exception("账号已存在");

        user.Email = input.Email;
        user.UserName = input.UserName;

        await DbContext.Users.Where(x => x.Id == UserContext.CurrentUserId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.Email, input.Email)
                    .SetProperty(y => y.UserName, input.UserName));

        await RefreshUserAsync(UserContext.CurrentUserId);
    }


    public async ValueTask EnableAsync(string id)
    {
        await DbContext.Users.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsDisabled, x => !x.IsDisabled));

        await RefreshUserAsync(UserContext.CurrentUserId);
    }

    public async ValueTask<bool> UpdateResidualCreditAsync(string id, long residualCredit)
    {
        var result = await DbContext.Users.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResidualCredit, y => y.ResidualCredit + residualCredit));

        await RefreshUserAsync(UserContext.CurrentUserId);

        return result > 0;
    }

    /// <summary>
    ///     修改密码
    /// </summary>
    public async ValueTask UpdatePasswordAsync(UpdatePasswordInput input)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == UserContext.CurrentUserId);
        if (user == null)
            throw new UnauthorizedAccessException();

        if (!user.VerifyPassword(input.OldPassword))
            throw new Exception("旧密码错误");

        user.SetPassword(input.NewPassword);

        await DbContext.Users.Where(x => x.Id == UserContext.CurrentUserId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Password, user.Password)
                .SetProperty(y => y.PasswordHas, user.PasswordHas));

        await RefreshUserAsync(UserContext.CurrentUserId);
    }

    public async Task<User?> RefreshUserAsync(string userId)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user != null)
        {
            await memoryCache.CreateAsync("User:" + userId, user);
        }

        return user;
    }

    [GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial Regex CheckUserName();

    [GeneratedRegex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$")]
    private static partial Regex CheEmail();
}