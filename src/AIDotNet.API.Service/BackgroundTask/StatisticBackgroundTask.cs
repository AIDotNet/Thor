using AIDotNet.Abstractions;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.BackgroundTask;

public sealed class StatisticBackgroundTask(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<LoggerDbContext>();
            // 获取今天的日期范围
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            // 查询统计数据
            var userStatistics = await dbContext.Loggers
                .Where(log =>
                    log.Type == ChatLoggerType.Consume && log.CreatedAt >= today && log.CreatedAt < tomorrow) // 今天的日志
                .GroupBy(log => new { log.UserId }) // 按用户ID和模型名称分组
                .Select(group => new
                {
                    UserId = group.Key.UserId,
                    RequestCount = group.Count(), // 请求次数
                    TotalQuota = group.Sum(log => log.Quota), // 消费额度
                    TotalTokens = group.Sum(log => log.PromptTokens + log.CompletionTokens) // Token总数
                })
                .ToListAsync(stoppingToken);

            // 统计用户请求 消费额度 Token总数
            foreach (var userStatistic in userStatistics)
            {
                // 删除今天的统计数据
                await dbContext.StatisticsConsumesNumbers
                    .Where(
                        x => x.Creator == userStatistic.UserId && x.Year == today.Year && x.Month == today.Month &&
                             x.Day == today.Day)
                    .ExecuteDeleteAsync(cancellationToken: stoppingToken);

                // 创建新的统计数据
                var statistics = new List<StatisticsConsumesNumber>()
                {
                    new()
                    {
                        Creator = userStatistic.UserId,
                        Year = (ushort)today.Year,
                        Month = (ushort)today.Month,
                        Day = (ushort)today.Day,
                        Type = StatisticsConsumesNumberType.Consumes,
                        Value = userStatistic.TotalQuota,
                    },
                    new()
                    {
                        Creator = userStatistic.UserId,
                        Year = (ushort)today.Year,
                        Month = (ushort)today.Month,
                        Day = (ushort)today.Day,
                        Type = StatisticsConsumesNumberType.Requests,
                        Value = userStatistic.RequestCount,
                    },
                    new()
                    {
                        Creator = userStatistic.UserId,
                        Year = (ushort)today.Year,
                        Month = (ushort)today.Month,
                        Day = (ushort)today.Day,
                        Type = StatisticsConsumesNumberType.Tokens,
                        Value = userStatistic.TotalTokens,
                    }
                };
                await dbContext.StatisticsConsumesNumbers.AddRangeAsync(statistics, stoppingToken);
            }


            var models = await dbContext.Loggers
                .Where(log =>
                    log.Type == ChatLoggerType.Consume && log.CreatedAt >= today && log.CreatedAt < tomorrow) // 今天的日志
                .GroupBy(log => new { log.UserId, log.ModelName }) // 按用户ID和模型名称分组
                .Select(group => new
                {
                    UserId = group.Key.UserId,
                    ModelName = group.Key.ModelName,
                    RequestCount = group.Count(), // 请求次数
                    TotalQuota = group.Sum(log => log.Quota), // 消费额度
                    TotalTokens = group.Sum(log => log.PromptTokens + log.CompletionTokens) // Token总数
                })
                .ToListAsync(stoppingToken);

            // 统计用户请求 消费额度 Token总数
            foreach (var model in models)
            {
                // 删除今天的统计数据
                await dbContext.ModelStatisticsNumbers
                    .Where(
                        x => x.Creator == model.UserId && x.ModelName == model.ModelName && x.Year == today.Year &&
                             x.Month == today.Month && x.Day == today.Day)
                    .ExecuteDeleteAsync(cancellationToken: stoppingToken);

                // 创建新的统计数据
                var statistics = new List<ModelStatisticsNumber>()
                {
                    new()
                    {
                        Creator = model.UserId,
                        ModelName = model.ModelName,
                        Year = (ushort)today.Year,
                        Month = (ushort)today.Month,
                        Day = (ushort)today.Day,
                        Count = model.RequestCount,
                        Quota = model.TotalQuota,
                        TokenUsed = model.TotalTokens
                    }
                };
                await dbContext.ModelStatisticsNumbers.AddRangeAsync(statistics, stoppingToken);
            }

            await dbContext.SaveChangesAsync(stoppingToken);

            // 休眠一定时间或直到下一次执行周期
            await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
        }
    }
}