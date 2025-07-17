using Microsoft.EntityFrameworkCore;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;
using Thor.Service.DataAccess;
using Thor.Service.Domain;
using Thor.Service.Exceptions;
using Thor.Service.Service;

namespace Thor.Service.BackgroundTask;

public sealed class AutoChannelDetectionBackgroundTask(
    IServiceProvider serviceProvider,
    ILogger<AutoChannelDetectionBackgroundTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // 获取环境变量是否启用自动检测通道
            var autoChannelDetection = Environment.GetEnvironmentVariable("AutoChannelDetection");

            if (autoChannelDetection?.ToLower() == "false")
            {
                logger.LogInformation("AutoChannelDetectionBackgroundTask: AutoChannelDetection is not enabled");
                return;
            }

            // 启动一分钟后开始执行，防止卡住启动
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);

            await Task.Factory.StartNew(() => AutoHandleExceptionChannelAsync(stoppingToken), stoppingToken,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);

            while (!stoppingToken.IsCancellationRequested)
            {
                var autoDisable = SettingService.GetBoolSetting(SettingExtensions.GeneralSetting.AutoDisableChannel);
                var interval = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.CheckInterval);
                if (interval <= 0)
                {
                    interval = 60;
                }

                logger.LogInformation(
                    $"AutoChannelDetectionBackgroundTask: AutoDisable: {autoDisable}, Interval: {interval}");

                if (autoDisable)
                {
                    await using var scope = serviceProvider.CreateAsyncScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<IThorContext>();
                    var channelService = scope.ServiceProvider.GetRequiredService<ChannelService>();
                    // 自动关闭通道
                    // 1. 获取启动自动检测通道
                    var channels = await dbContext.Channels.Where(x => x.ControlAutomatically && x.Disable == false)
                        .ToListAsync(cancellationToken: stoppingToken);

                    // 2. 对于获取的渠道进行检测
                    foreach (var channel in channels)
                    {
                        await TestChannelAsync(channel, channelService, dbContext).ConfigureAwait(false);
                    }
                }

                // 默认单位（分钟）
                await Task.Delay((1000 * 60) * interval, stoppingToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $"AutoChannelDetectionBackgroundTask Error: {e.Message}");
        }
    }

    /// <summary>
    /// 自动处理异常通道
    /// </summary>
    private async Task AutoHandleExceptionChannelAsync(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IThorContext>();
        var channelService = scope.ServiceProvider.GetRequiredService<ChannelService>();
        while (stoppingToken.IsCancellationRequested == false)
        {
            // 对于异常通道，每15秒检测一次，以便渠道快速恢复。
            await Task.Delay((15000), stoppingToken);

            try
            {
                foreach (var channel in await dbContext.Channels
                             .AsNoTracking()
                             .Where(x => x.ControlAutomatically && x.Disable)
                             .ToArrayAsync(cancellationToken: stoppingToken))
                {
                    await TestChannelAsync(channel, channelService, dbContext);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"AutoChannelDetectionBackgroundTask Error: {e.Message}");
            }
        }
    }

    private async Task TestChannelAsync(ChatChannel channel, ChannelService channelService, IThorContext dbContext)
    {
        // 标记是否需要清除缓存
        bool needClearCache = false;

        try
        {
            // 3. 检测通道是否需要关闭
            var (succeed, timeout) = await channelService.TestChannelAsync(channel.Id);
            // 如果检测成功并且通道当前是禁用状态，则启用
            if (succeed)
            {
                // 只有当渠道当前是禁用状态时才需要启用并清除缓存
                var updateResult = await dbContext.Channels.Where(x => x.Id == channel.Id && x.Disable == true)
                    .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, false));

                if (updateResult > 0)
                {
                    logger.LogInformation("AutoChannelDetectionBackgroundTask: 渠道 {ChannelId} 从禁用状态恢复为启用状态", channel.Id);
                    needClearCache = true; // 状态发生变化，需要清除缓存
                }
                // 如果updateResult = 0，说明渠道本身就是启用状态，无需任何操作
            }
            else
            {
                logger.LogWarning(
                    $"AutoChannelDetectionBackgroundTask: Channel {channel.Id} is timeout: {timeout}");
                // 只有当渠道当前是启用状态时才需要禁用并清除缓存
                var updateResult = await dbContext.Channels.Where(x => x.Id == channel.Id && x.Disable == false)
                    .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, true));

                if (updateResult > 0)
                {
                    logger.LogWarning("AutoChannelDetectionBackgroundTask: 渠道 {ChannelId} 从启用状态变更为禁用状态，原因：{Reason}",
                        channel.Id, timeout > 0 ? $"超时{timeout}ms" : "检测失败");
                    needClearCache = true; // 状态发生变化，需要清除缓存
                }
                // 如果updateResult = 0，说明渠道本身就是禁用状态，无需任何操作
            }
        }
        catch (ChannelException e)
        {
            logger.LogError(e, $"AutoChannelDetectionBackgroundTask Error: {e.Message}");
            // 只有当渠道当前是启用状态时才需要禁用并清除缓存
            var updateResult = await dbContext.Channels.Where(x => x.Id == channel.Id && x.Disable == false)
                .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, true));

            if (updateResult > 0)
            {
                logger.LogWarning("AutoChannelDetectionBackgroundTask: 渠道 {ChannelId} 因异常从启用状态变更为禁用状态，异常：{Exception}",
                    channel.Id, e.Message);
                needClearCache = true; // 状态发生变化，需要清除缓存
            }
            // 如果updateResult = 0，说明渠道本身就是禁用状态，无需任何操作
        }
        catch (Exception e)
        {
            logger.LogError(e, $"AutoChannelDetectionBackgroundTask Error: {e.Message}");
            // 只有当渠道当前是启用状态时才需要禁用并清除缓存
            var updateResult = await dbContext.Channels.Where(x => x.Id == channel.Id && x.Disable == false)
                .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, true));

            if (updateResult > 0)
            {
                logger.LogWarning("AutoChannelDetectionBackgroundTask: 渠道 {ChannelId} 因异常从启用状态变更为禁用状态，异常：{Exception}",
                    channel.Id, e.Message);
                needClearCache = true; // 状态发生变化，需要清除缓存
            }
            // 如果updateResult = 0，说明渠道本身就是禁用状态，无需任何操作
        }
        finally
        {
            // 如果需要清除缓存，则在finally块中执行，确保总是被执行
            if (needClearCache)
            {
                await ClearChannelCacheAsync();
            }
        }
    }

    /// <summary>
    /// 清除渠道缓存，确保渠道状态变更立即生效
    /// </summary>
    private async Task ClearChannelCacheAsync()
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var cache = scope.ServiceProvider.GetRequiredService<IServiceCache>();
            await cache.RemoveAsync("CacheKey:Channel");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "清除渠道缓存时发生错误：{ErrorMessage}", ex.Message);
        }
    }
}