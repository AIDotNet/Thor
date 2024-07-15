using Microsoft.EntityFrameworkCore;
using Thor.Service.DataAccess;
using Thor.Service.Service;

namespace Thor.Service.BackgroundTask;

public sealed class LoggerBackgroundTask(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
            var dbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();

            var today = DateTime.Now.Date;

            var day = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.IntervalDays);

            if (day <= 0)
            {
                day = 30;
            }

            var deleteDate = today.AddDays(-day);

            await dbContext.Loggers
                .Where(log => log.CreatedAt < deleteDate)
                .ExecuteDeleteAsync(cancellationToken: stoppingToken);
        }
    }
}