using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain.Core;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public static class StatisticsService
{
    public static async ValueTask<StatisticsDto> GetStatisticsAsync(LoggerDbContext dbContext,
        AIDotNetDbContext aiDotNetDbContext,
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

        // 获取七天的日期范围
        var today = DateTime.Now.Date;

        var userQuery = dbContext.StatisticsConsumesNumbers
            .Where(log => log.Year == today.Year && log.Month == today.Month && log.Day >= today.Day - 7); // 七天的日志

        if (!userContext.IsAdmin)
        {
            userQuery = userQuery.Where(log => log.Creator == userContext.CurrentUserId);
        }

        // 查询统计数据总和
        var userStatistics =
            userQuery
                .GroupBy(log => new { log.Year, log.Month, log.Day, log.Type }) // 按用户ID和模型名称分组
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Day = group.Key.Day,
                    Type = group.Key.Type,
                    Value = group.Sum(log => log.Value), // 请求次数
                });

        // 统计用户请求 消费额度 Token总数
        foreach (var userStatistic in (await userStatistics.ToListAsync()))
        {
            switch (userStatistic.Type)
            {
                case StatisticsConsumesNumberType.Consumes:
                    statisticsDto.Consumes.Add(new StatisticsNumberDto
                    {
                        DateTime =
                            new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day).ToString(
                                "yyyy-MM-dd"),
                        Value = userStatistic.Value
                    });
                    break;
                case StatisticsConsumesNumberType.Requests:
                    statisticsDto.Requests.Add(new StatisticsNumberDto
                    {
                        DateTime =
                            new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day).ToString(
                                "yyyy-MM-dd"),
                        Value = userStatistic.Value
                    });
                    break;
                case StatisticsConsumesNumberType.Tokens:
                    statisticsDto.Tokens.Add(new StatisticsNumberDto
                    {
                        DateTime =
                            new DateTime(userStatistic.Year, userStatistic.Month, userStatistic.Day).ToString(
                                "yyyy-MM-dd"),
                        Value = userStatistic.Value
                    });
                    break;
            }
        }

        // 统计用户的模型数据

        var query = dbContext.ModelStatisticsNumbers
            .Where(log => log.Year == today.Year && log.Month == today.Month && log.Day >= today.Day - 7); // 七天的日志

        if (!userContext.IsAdmin)
        {
            query = query.Where(log => log.Creator == userContext.CurrentUserId);
        }

        var modelStatistics = await
            query
                .GroupBy(log => new { log.ModelName, log.Year, log.Month, log.Day }) // 按模型名称分组
                .Select(group => new
                {
                    CreateAt = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day),
                    ModelName = group.Key.ModelName,
                    Count = group.Count(), // 请求次数
                    TokenUsed = group.Sum(log => log.TokenUsed), // Token使用量
                    Quota = group.Sum(log => log.Quota) // 消耗额度
                }).ToListAsync();


        statisticsDto.ModelDate = modelStatistics.Select(x => x.CreateAt.ToString("MM-dd")).Distinct().ToList();

        foreach (var modelStatistic in modelStatistics.GroupBy(x => new
                 {
                     x.ModelName,
                     x.CreateAt
                 }))
        {
            if (statisticsDto.Models.All(x => x.Name != modelStatistic.Key.ModelName))
            {
                statisticsDto.Models.Add(new ModelStatisticsDto
                {
                    CreatedAt = modelStatistic.Key.CreateAt,
                    Name = modelStatistic.Key.ModelName,
                    Data = new List<int>()
                });
            }

            var model = statisticsDto.Models.FirstOrDefault(x => x.Name == modelStatistic.Key.ModelName);

            model!.Data.Add(modelStatistic.Sum(x => x.Quota));
            model.TokenUsed = modelStatistic.Sum(x => x.TokenUsed);
        }

        #endregion

        // 统计用户请求 消费额度 Token总数
        statisticsDto.CurrentConsumedCredit = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || x.Creator == userContext.CurrentUserId &&
                x.Type == StatisticsConsumesNumberType.Consumes)
            .SumAsync(log => log.Value);

        statisticsDto.CurrentResidualCredit = (await aiDotNetDbContext.Users
            .Where(x => x.Id == userContext.CurrentUserId)
            .FirstOrDefaultAsync())?.ResidualCredit ?? 0;

        statisticsDto.TotalRequestCount = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || x.Creator == userContext.CurrentUserId &&
                x.Type == StatisticsConsumesNumberType.Requests)
            .SumAsync(log => log.Value);

        statisticsDto.TotalTokenCount = await dbContext.StatisticsConsumesNumbers
            .Where(x => userContext.IsAdmin || x.Creator == userContext.CurrentUserId &&
                x.Type == StatisticsConsumesNumberType.Tokens)
            .SumAsync(log => log.Value);

        return statisticsDto;
    }
}