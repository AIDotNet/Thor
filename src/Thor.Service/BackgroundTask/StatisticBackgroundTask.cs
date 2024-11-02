using Thor.Core.DataAccess;
using Thor.Service.Domain.Core;

namespace Thor.Service.BackgroundTask;

public sealed class StatisticBackgroundTask(IServiceProvider serviceProvider, ILogger<StatisticBackgroundTask> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // 启动一分钟后开始执行，防止卡住启动
            await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await HandlerUserStatisticsAsync(stoppingToken);

                // 休眠一定时间或直到下一次执行周期
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $"StatisticBackgroundTask Error: {e.Message}");
        }
    }

    private async Task HandlerUserStatisticsAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ILoggerDbContext>();
        // 获取今天的日期范围
        var today = DateTime.Now.Date;
        var tomorrow = today.AddDays(1);

        // 统计用户请求 消费额度 Token总数
        foreach (var userStatistic in  await dbContext.Loggers
                     .Where(log =>
                         log.Type == ThorChatLoggerType.Consume && log.CreatedAt >= today &&
                         log.CreatedAt < tomorrow) // 今天的日志
                     .GroupBy(log => new { log.UserId }) // 按用户ID和模型名称分组
                     .Select(group => new
                     {
                         UserId = group.Key.UserId,
                         RequestCount = group.Count(), // 请求次数
                         TotalQuota = group.Sum(log => log.Quota), // 消费额度
                         TotalTokens = group.Sum(log => log.PromptTokens + log.CompletionTokens) // Token总数
                     })
                     .ToArrayAsync(stoppingToken))
        {
            // 删除今天的统计数据
            await dbContext.StatisticsConsumesNumbers
                .Where(
                    x => x.Creator == userStatistic.UserId && x.Year == today.Year &&
                         x.Month == today.Month &&
                         x.Day == today.Day)
                .ExecuteDeleteAsync(cancellationToken: stoppingToken);

            // 创建新的统计数据
            var statistics = new List<StatisticsConsumesNumber>()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Creator = userStatistic.UserId,
                    Year = (int)today.Year,
                    Month = (int)today.Month,
                    Day = (int)today.Day,
                    Type = StatisticsConsumesNumberType.Consumes,
                    Value = userStatistic.TotalQuota,
                },
                new()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Creator = userStatistic.UserId,
                    Year = (int)today.Year,
                    Month = (int)today.Month,
                    Day = (int)today.Day,
                    Type = StatisticsConsumesNumberType.Requests,
                    Value = userStatistic.RequestCount,
                },
                new()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Creator = userStatistic.UserId,
                    Year = (int)today.Year,
                    Month = (int)today.Month,
                    Day = (int)today.Day,
                    Type = StatisticsConsumesNumberType.Tokens,
                    Value = userStatistic.TotalTokens,
                }
            };
            await dbContext.StatisticsConsumesNumbers.AddRangeAsync(statistics, stoppingToken);
        }

        // 统计用户请求 消费额度 Token总数
        foreach (var model in  await dbContext.Loggers
                     .Where(log =>
                         log.Type == ThorChatLoggerType.Consume && log.CreatedAt >= today &&
                         log.CreatedAt < tomorrow) // 今天的日志
                     .GroupBy(log => new { log.UserId, log.ModelName }) // 按用户ID和模型名称分组
                     .Select(group => new
                     {
                         UserId = group.Key.UserId,
                         ModelName = group.Key.ModelName,
                         RequestCount = group.Count(), // 请求次数
                         TotalQuota = group.Sum(log => log.Quota), // 消费额度
                         TotalTokens = group.Sum(log => log.PromptTokens + log.CompletionTokens) // Token总数
                     })
                     .ToArrayAsync(cancellationToken: stoppingToken))
        {
            // 删除今天的统计数据
            await dbContext.ModelStatisticsNumbers
                .Where(
                    x => x.Creator == model.UserId && x.ModelName == model.ModelName &&
                         x.Year == today.Year &&
                         x.Month == today.Month && x.Day == today.Day)
                .ExecuteDeleteAsync(cancellationToken: stoppingToken);

            // 创建新的统计数据
            var statistics = new List<ModelStatisticsNumber>()
            {
                new()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Creator = model.UserId,
                    ModelName = model.ModelName,
                    Year = (int)today.Year,
                    Month = (int)today.Month,
                    Day = (int)today.Day,
                    Count = model.RequestCount,
                    Quota = (int)model.TotalQuota,
                    TokenUsed = model.TotalTokens
                }
            };
            await dbContext.ModelStatisticsNumbers.AddRangeAsync(statistics, stoppingToken);
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}