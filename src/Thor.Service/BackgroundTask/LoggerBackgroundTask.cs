using Microsoft.EntityFrameworkCore;
using Thor.Core.DataAccess;
using Thor.Service.DataAccess;
using Thor.Service.Service;

namespace Thor.Service.BackgroundTask;

public sealed class LoggerBackgroundTask(
    IServiceProvider serviceProvider,
    ILogger<LoggerBackgroundTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!SettingService.GetBoolSetting(SettingExtensions.GeneralSetting.EnableClearLog))
                {
                    Console.WriteLine("日志清理功能已关闭");
                    return;
                }

                // TODO: 每小时执行一次
                await Task.Delay(1000 * 60 * 60, stoppingToken);

                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ILoggerDbContext>();

                var today = DateTime.Now.Date;

                var day = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.IntervalDays);

                if (day <= 0)
                {
                    day = 30;
                }

                var deleteDate = today.AddDays(-day);

                var result = await dbContext.Loggers
                    .Where(log => log.CreatedAt < deleteDate)
                    .ExecuteDeleteAsync(cancellationToken: stoppingToken);

                logger.LogInformation($"删除日志成功，删除时间：{deleteDate} 删除数量：{result}");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $"LoggerBackgroundTask Error: {e.Message}");
        }
    }
}