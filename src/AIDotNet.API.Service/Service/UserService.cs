using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public class UserService(
    IServiceProvider serviceProvider,
    LoggerService loggerService,
    TokenService tokenService)
    : ApplicationService(serviceProvider)
{
    public async ValueTask<User> CreateAsync(CreateUserInput input)
    {
        // 判断是否存在
        var exist = await DbContext.Users.AnyAsync(x => x.UserName == input.UserName || x.Email == input.Email);
        if (exist)
        {
            throw new Exception("用户名已存在");
        }

        var user = new User(Guid.NewGuid().ToString(), input.UserName, input.Email, input.Password);

        // 初始用户额度
        var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
        user.SetResidualCredit(userQuota);

        await DbContext.Users.AddAsync(user);

        await tokenService.CreateAsync(new TokenInput()
        {
            Name = "默认Token",
            UnlimitedQuota = true,
            UnlimitedExpired = true,
        }); 

        await loggerService.CreateSystemAsync("创建用户：" + user.UserName);
        
        return user;
    }

    public async ValueTask<User?> GetAsync()
    {
        var info = await DbContext.Users.FindAsync(UserContext.CurrentUserId);

        if (info == null)
            throw new UnauthorizedAccessException();

        return info;
    }

    public async ValueTask<bool> RemoveAsync(string id)
    {
        if (UserContext.CurrentUserId == id)
            throw new Exception("不能删除自己");

        var result = await DbContext.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
        return result > 0;
    }

    public async ValueTask<PagingDto<User>> GetAsync(int page, int pageSize, string? keyword)
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

    public async ValueTask<bool> ConsumeAsync(string id, long consume, int consumeToken, string? token)
    {
        var result = await DbContext
            .Users
            .Where(x => x.Id == id && x.ResidualCredit >= consume)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.ResidualCredit, y => y.ResidualCredit - consume)
                    .SetProperty(y => y.RequestCount, y => y.RequestCount + 1)
                    .SetProperty(y => y.ConsumeToken, y => y.ConsumeToken + consumeToken));

        if (!string.IsNullOrEmpty(token))
        {
            await DbContext
                .Tokens.Where(x => x.Key == token)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.RemainQuota, y => y.RemainQuota - consume)
                        .SetProperty(y => y.AccessedTime, DateTime.Now)
                        .SetProperty(y => y.UsedQuota, y => y.UsedQuota + consume));
        }

        return result > 0;
    }

    public async ValueTask UpdateAsync(UpdateUserInput input)
    {
        if (await DbContext.Users.AnyAsync(x =>
                x.Id != UserContext.CurrentUserId && x.Email == input.Email))
            throw new Exception("用户名或邮箱已存在");

        await DbContext.Users.Where(x => x.Id == UserContext.CurrentUserId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.Email, input.Email)
                    .SetProperty(y => y.Avatar, input.Avatar));
    }

    public async ValueTask EnableAsync(string id)
    {
        await DbContext.Users.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsDisabled, x => !x.IsDisabled));
    }

    public async ValueTask<bool> UpdateResidualCreditAsync(string id, long residualCredit)
    {
        var result = await DbContext.Users.Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResidualCredit, y => y.ResidualCredit + residualCredit));

        return result > 0;
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public async ValueTask UpdatePasswordAsync(UpdatePasswordInput input)
    {
        var user = await DbContext.Users.FindAsync(UserContext.CurrentUserId);
        if (user == null)
            throw new UnauthorizedAccessException();

        if (!user.VerifyPassword(input.OldPassword))
            throw new Exception("旧密码错误");

        user.SetPassword(input.NewPassword);

        await DbContext.Users.Where(x => x.Id == UserContext.CurrentUserId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Password, user.Password)
                .SetProperty(y => y.PasswordHas, user.PasswordHas));
    }
}