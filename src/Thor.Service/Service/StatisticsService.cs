using Thor.Core.DataAccess;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public static class StatisticsService
{
    public static async ValueTask<StatisticsDto> GetStatisticsAsync(ILoggerDbContext dbContext,
        IThorContext aiDotNetDbContext,
        IUserContext userContext)
    {
        var statisticsDto = new StatisticsDto
        {
            Consumes = [],
            Requests = [],
            Tokens = [],
            Models = [],
            ModelDate = []
        };

        #region 统计数据

        var sevenDaysAgo = DateTime.Now.Date.AddDays(-7);

        if (userContext.IsAdmin)
        {
            var result = await dbContext.Loggers.Where(x =>
                    (x.Type == ThorChatLoggerType.CreateUser || x.Type == ThorChatLoggerType.Recharge) &&
                    x.CreatedAt > sevenDaysAgo)
                .ToListAsync();

            var user = result.Where(x => x.Type == ThorChatLoggerType.CreateUser)
                .GroupBy(x => x.CreatedAt.ToString("yyyy-MM-dd"))
                .Select(x => new StatisticsNumberDto
                {
                    Name = x.Key,
                    Value = x.Count()
                });
            var recharge = result.Where(x => x.Type == ThorChatLoggerType.Recharge)
                .GroupBy(x => x.CreatedAt.ToString("yyyy-MM-dd"))
                .Select(x => new StatisticsNumberDto
                {
                    Name = x.Key,
                    Value = x.Sum(x => x.Quota)
                });

            statisticsDto.UserNewData = new List<StatisticsNumberDto>();
            statisticsDto.RechargeData = new List<StatisticsNumberDto>();

            foreach (var dateTime in Enumerable.Range(0, 7).Select(days => DateTime.Now.Date.AddDays(-days))
                         .Order()
                         .ToList())
            {
                var item = user.FirstOrDefault(x => x.Name == dateTime.ToString("yyyy-MM-dd"));
                var rechargeItem = recharge.FirstOrDefault(x => x.Name == dateTime.ToString("yyyy-MM-dd"));
                if (item == null)
                {
                    statisticsDto.UserNewData.Add(new StatisticsNumberDto()
                    {
                        Name = dateTime.ToString("yyyy-MM-dd"),
                        Value = 0
                    });
                }
                else
                {
                    statisticsDto.UserNewData.Add(item);
                }

                if (rechargeItem == null)
                {
                    statisticsDto.RechargeData.Add(new StatisticsNumberDto()
                    {
                        Name = dateTime.ToString("yyyy-MM-dd"),
                        Value = 0
                    });
                }
                else
                {
                    statisticsDto.RechargeData.Add(rechargeItem);
                }
            }
        }


        var userQuery = dbContext.StatisticsConsumesNumbers
            .Where(log => log.CreatedAt >= sevenDaysAgo);

        if (!userContext.IsAdmin) userQuery = userQuery.Where(log => log.Creator == userContext.CurrentUserId);

        // 查询统计数据总和
        var userStatistics = await userQuery
                .GroupBy(log => new { log.Year, log.Month, log.Day, log.Type }) // 按用户ID和模型名称分组
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    group.Key.Day,
                    group.Key.Type,
                    Value = group.Sum(log => log.Value) // 请求次数
                }).ToListAsync()
            ;
        // 获取过去七天的日期列表
        var dateList = Enumerable.Range(0, 7).Select(days => DateTime.Now.Date.AddDays(-days))
            .Order()
            .ToList();

        // 统计用户请求 消费额度 Token总数
        foreach (var date in dateList)
            if (userStatistics
                .Any(stat => new DateTime(stat.Year, stat.Month, stat.Day) == date))
            {
                foreach (var userStatistic in userStatistics
                             .Where(stat => new DateTime(stat.Year, stat.Month, stat.Day) == date))
                    switch (userStatistic.Type)
                    {
                        case StatisticsConsumesNumberType.Consumes:
                            statisticsDto.Consumes.Add(new StatisticsNumberDto
                            {
                                DateTime =
                                    new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day)
                                        .ToString(
                                            "yyyy-MM-dd"),
                                Value = userStatistic.Value
                            });
                            break;
                        case StatisticsConsumesNumberType.Requests:
                            statisticsDto.Requests.Add(new StatisticsNumberDto
                            {
                                DateTime =
                                    new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day)
                                        .ToString(
                                            "yyyy-MM-dd"),
                                Value = userStatistic.Value
                            });
                            break;
                        case StatisticsConsumesNumberType.Tokens:
                            statisticsDto.Tokens.Add(new StatisticsNumberDto
                            {
                                DateTime =
                                    new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day)
                                        .ToString(
                                            "yyyy-MM-dd"),
                                Value = userStatistic.Value
                            });
                            break;
                    }
            }
            else
            {
                statisticsDto.Consumes.Add(new StatisticsNumberDto
                {
                    DateTime = date.ToString("yyyy-MM-dd"),
                    Value = 0
                });
                statisticsDto.Requests.Add(new StatisticsNumberDto
                {
                    DateTime = date.ToString("yyyy-MM-dd"),
                    Value = 0
                });
                statisticsDto.Tokens.Add(new StatisticsNumberDto
                {
                    DateTime = date.ToString("yyyy-MM-dd"),
                    Value = 0
                });
            }

        // 统计用户的模型数据

        var query = dbContext.ModelStatisticsNumbers
            .Where(log => log.CreatedAt >= sevenDaysAgo); // 七天的日志

        if (!userContext.IsAdmin) query = query.Where(log => log.Creator == userContext.CurrentUserId);

        var modelStatistics = await
            query
                .GroupBy(log => new { log.ModelName, log.Year, log.Month, log.Day }) // 按模型名称分组
                .Select(group => new
                {
                    CreateAt = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day),
                    group.Key.ModelName,
                    Count = group.Count(), // 请求次数
                    TokenUsed = group.Sum(log => log.TokenUsed), // Token使用量
                    Quota = group.Sum(log => log.Quota) // 消耗额度
                }).ToListAsync();


        var allDates = dateList.Select(x => x.ToString("yyyy-MM-dd")).Distinct().ToList();

        statisticsDto.ModelDate = allDates;

        foreach (var modelStatistic in modelStatistics.GroupBy(x => new
                 {
                     x.ModelName,
                     x.CreateAt
                 }))
        {
            if (statisticsDto.Models.All(x => x.Name != modelStatistic.Key.ModelName))
            {
                var dataForAllDates = new List<int>();

                // Initialize the data list with 0 for all dates
                foreach (var date in allDates) dataForAllDates.Add(0);

                statisticsDto.Models.Add(new ModelStatisticsDto
                {
                    CreatedAt = modelStatistic.Key.CreateAt.ToString("yyyy-MM-dd"),
                    Name = modelStatistic.Key.ModelName,
                    Data = dataForAllDates
                });
            }

            var model = statisticsDto.Models.FirstOrDefault(x => x.Name == modelStatistic.Key.ModelName);

            // Find the index of the current date in the allDates list
            var dateIndex = allDates.IndexOf(modelStatistic.Key.CreateAt.ToString("yyyy-MM-dd"));

            model.TokenUsed = modelStatistic.Sum(x => x.TokenUsed);

            if (dateIndex == -1 || dateIndex >= model.Data.Count)
                continue;

            try
            {
                model!.Data[dateIndex] = modelStatistic.Sum(x => x.Quota);
            }
            catch
            {
            }
        }

        // 根据modelStatistics的数据，统计模型消费额度排名
        var modelRanking = modelStatistics
            .GroupBy(x => x.ModelName)
            .Select(group => new
            {
                ModelName = group.Key,
                Quota = group.Sum(x => x.Quota)
            })
            .OrderByDescending(x => x.Quota)
            .Take(10)
            .Select(x => new ModelRankingDto
            {
                Value = x.Quota,
                Name = x.ModelName
            })
            .ToList();

        statisticsDto.ModelRanking = modelRanking;

        #endregion

        statisticsDto.Consumes = statisticsDto.Consumes.OrderBy(x => x.DateTime)
            .ThenByDescending(x => x.Value)
            .ToList();

        // 消费总额
        statisticsDto.CurrentConsumedCredit = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || (x.Creator == userContext.CurrentUserId &&
                                                x.Type == StatisticsConsumesNumberType.Consumes))
            .SumAsync(log => log.Value);

        // 当前剩余额度
        statisticsDto.CurrentResidualCredit = (await aiDotNetDbContext.Users
            .Where(x => x.Id == userContext.CurrentUserId)
            .FirstOrDefaultAsync())?.ResidualCredit ?? 0;

        // 总请求次数
        statisticsDto.TotalRequestCount = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || (x.Creator == userContext.CurrentUserId &&
                                                x.Type == StatisticsConsumesNumberType.Requests))
            .CountAsync();

        // 总Token数
        statisticsDto.TotalTokenCount = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || (x.Creator == userContext.CurrentUserId &&
                                                x.Type == StatisticsConsumesNumberType.Tokens))
            .SumAsync(log => log.Value);

        return statisticsDto;
    }
}