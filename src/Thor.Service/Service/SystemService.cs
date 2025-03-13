using Thor.Abstractions.Exceptions;
using Thor.Infrastructure;
using Thor.Service.Infrastructure;
using Thor.Service.Options;

namespace Thor.Service.Service;

/// <summary>
/// 系统服务
/// </summary>
/// <param name="serviceProvider"></param>
public sealed class SystemService(
    IServiceProvider serviceProvider,
    IServiceCache serviceCache,
    LoggerService loggerService)
    : ApplicationService(serviceProvider), IScopeDependency
{
    /// <summary>
    /// 触发分享获取奖励
    /// </summary>
    /// <returns></returns>
    public async ValueTask InviteCode(string userId, User newUser)
    {
        if (ChatCoreOptions.Invite == null || !ChatCoreOptions.Invite.Enable)
            throw new Exception("邀请功能未开启，请联系管理员");

        var logger = GetLogger<SystemService>();

        var user = await DbContext.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("用户不存在");

        // 一星期内
        var span = TimeSpan.FromDays(7);

        // 获取当前用户的分享缓存
        var key = $"share:{userId}";
        var cache = await serviceCache.GetOrCreateAsync(key, async () => await Task.FromResult(1),
            span);

        if (cache >= ChatCoreOptions.Invite.Limit)
        {
            logger.LogWarning("用户" + user.UserName + "邀请次数已达上限，邀请限制为每一星期" + ChatCoreOptions.Invite.Limit + "次");
            return;
        }

        await DbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(x => x.ResidualCredit, x => x.ResidualCredit + ChatCoreOptions.Invite.Credit));

        await loggerService.CreateRechargeAsync($"恭喜：{user.UserName}邀请新用户" + newUser.UserName + "获得额度:" +
                                                RenderHelper.RenderQuota(ChatCoreOptions.Invite.Credit, 6) +
                                                ",邀请码剩余次数：" + cache,
            ChatCoreOptions.Invite.Credit, userId);

        await serviceCache.IncrementAsync(key);

        logger.LogInformation("用户" + user.UserName + "邀请成功，当前邀请次数：" + cache);
    }

    /// <summary>
    /// 返回邀请信息
    /// </summary>
    /// <returns></returns>
    public async Task<object> InviteInfo()
    {
        if (ChatCoreOptions.Invite == null || !ChatCoreOptions.Invite.Enable)
        {
            return new
            {
                credit = 0,
                count = 0,
                limit = 0
            };
        }

        var key = $"share:{UserContext.CurrentUserId}";
        var cache = await serviceCache.GetOrCreateAsync(key, async () => await Task.FromResult(1),
            TimeSpan.FromDays(7));

        return new
        {
            credit = ChatCoreOptions.Invite.Credit,
            count = cache,
            limit = ChatCoreOptions.Invite.Limit
        };
    }
}