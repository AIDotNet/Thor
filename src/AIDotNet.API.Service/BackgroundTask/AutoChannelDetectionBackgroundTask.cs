using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Service;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.BackgroundTask;

public sealed class AutoChannelDetectionBackgroundTask(
    IServiceProvider serviceProvider,
    ILogger<AutoChannelDetectionBackgroundTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
                await using (var scope = serviceProvider.CreateAsyncScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AIDotNetDbContext>();
                    var channelService = scope.ServiceProvider.GetRequiredService<ChannelService>();
                    // 自动关闭通道
                    // 1. 获取启动自动检测通道
                    var channels = await dbContext.Channels.Where(x => x.ControlAutomatically)
                        .ToListAsync(cancellationToken: stoppingToken);
                    // 2. 对于获取的渠道进行检测
                    foreach (var channel in channels)
                    {
                        try
                        {
                            // 3. 检测通道是否需要关闭
                            var (succeed, timeout) = await channelService.TestChannelAsync(channel.Id);
                            // 如果检测成功并且通道未关闭则更新状态
                            if (succeed && channel.Disable == false)
                            {
                                logger.LogWarning($"AutoChannelDetectionBackgroundTask: Channel {channel.Id} is succeed.");
                                await dbContext.Channels.Where(x => x.Id == channel.Id)
                                    .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, false),
                                        cancellationToken: stoppingToken);
                            }
                            else
                            {
                                logger.LogWarning(
                                    $"AutoChannelDetectionBackgroundTask: Channel {channel.Id} is timeout: {timeout}");
                                // 5. 如果通道超时则关闭
                                await dbContext.Channels.Where(x => x.Id == channel.Id)
                                    .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, true),
                                        cancellationToken: stoppingToken);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, $"AutoChannelDetectionBackgroundTask Error: {e.Message}");
                            // 5. 如果通道超时则关闭
                            await dbContext.Channels.Where(x => x.Id == channel.Id)
                                .ExecuteUpdateAsync(item => item.SetProperty(x => x.Disable, true),
                                    cancellationToken: stoppingToken);
                        }
                    }
                }
            }

            // 默认单位（分钟）
            await Task.Delay((1000 * 60) * interval, stoppingToken);
        }
    }
}