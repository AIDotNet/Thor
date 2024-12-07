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
    : ApplicationService(serviceProvider) ,IScopeDependency
{
    /// <summary>
    /// 触发分享获取奖励
    /// </summary>
    /// <returns></returns>
    public async ValueTask ShareAsync(string userId, HttpContext context)
    {
        if (ChatCoreOptions.Shared == null || !ChatCoreOptions.Shared.EnableShareAd)
            throw new Exception("分享功能未开启");

        var logger = GetLogger<SystemService>();

        var user = await DbContext.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("用户不存在");

        // 获取当前时间到今天结束的时间
        var now = DateTime.Now;
        var end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        var span = end - now;

        // 获取当前用户的分享缓存
        var key = $"share:{userId}";
        var cache = await serviceCache.GetOrCreateAsync(key, async () => await Task.FromResult(1),
            span);

        if (cache >= ChatCoreOptions.Shared.ShareLimit)
        {
            logger.LogWarning("用户" + user.UserName + "分享次数已达上限");
            return;
        }

        // 获取ip
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var header))
        {
            ip = header;
        }

        // 判断当前ip是否已经获取过
        var ipKey = $"share:{userId}:{ip}";
        var ipCache = await serviceCache.GetAsync<string>(ipKey);

        if (!string.IsNullOrWhiteSpace(ipCache))
        {
            logger.LogWarning("用户" + user.UserName + "分享ip:" + ip + "已经获取过奖励");
            return;
        }

        await DbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(x => x.ResidualCredit, x => x.ResidualCredit + ChatCoreOptions.Shared.ShareCredit));

        // 设置ip缓存
        await serviceCache.CreateAsync(ipKey, ip, span);

        await loggerService.CreateRechargeAsync("获得分享奖励 用户" + user.UserName + " 分享点击ip:" + ip + " 额度:" +
                                                RenderHelper.RenderQuota(ChatCoreOptions.Shared.ShareCredit, 6),
            ChatCoreOptions.Shared.ShareCredit, userId);

        await serviceCache.IncrementAsync(key);

        logger.LogInformation("用户" + user.UserName + "分享成功");
    }
}