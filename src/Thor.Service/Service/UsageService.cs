using Thor.Abstractions.Dtos;
using Thor.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public class UsageService(ILoggerDbContext dbContext, IUserContext userContext)
{
    // 已知的API服务类型及其对应的URL前缀
    private static readonly Dictionary<string, string> KnownApiServices = new()
    {
        { "聊天完成", "/v1/chat/completions" },
        { "图片服务", "/v1/images/generations" }, // 代表整组图片相关服务
        { "嵌入服务", "/v1/embeddings" },
        { "Audio Speeches", "/v1/organization/usage/audio_speeches" },
        { "Audio Transcriptions", "/v1/organization/usage/audio_transcriptions" },
        { "音频服务", "/v1/audio/speech" }, // 代表整组音频相关服务
    };

    public async Task<UsageResponse> GetUsageAsync(string? token, DateTime? startDate, DateTime? endDate)
    {
        // 设置默认日期范围，如果未提供则使用过去15天
        startDate ??= DateTime.Now.Date.AddDays(-15);
        endDate = endDate == null ? DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59) : // 今天结束 (23:59:59)
            endDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // 结束日期的结束时间

        // 查询符合条件的聊天日志
        var query = dbContext.Loggers
            .Where(x => x.UserId == userContext.CurrentUserId)
            .Where(l => string.IsNullOrEmpty(token) || l.TokenName == token)
            .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
            .AsNoTracking();

        // 获取所有符合条件的日志记录
        var logs = await query.ToListAsync();

        // 创建响应对象
        var response = new UsageResponse
        {
            TotalCost = logs.Sum(l => l.Quota),
            TotalRequestCount = logs.Count,
            TotalTokenCount = logs.Sum(l => l.PromptTokens + l.CompletionTokens)
        };

        // 生成日期范围（从开始日期到结束日期的每一天）
        var dateRange = GetDateRange(startDate.Value, endDate.Value);

        // 按天分组并计算每天的使用情况
        var dailyUsageGroups = logs
            .GroupBy(l => l.CreatedAt.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 填充每日消费记录，确保每天都有记录
        foreach (var date in dateRange)
        {
            var dailyDto = new DailyUsageDto
            {
                Date = date,
                Cost = 0,
                RequestCount = 0,
                TokenCount = 0
            };

            if (dailyUsageGroups.TryGetValue(date, out var dayLogs))
            {
                dailyDto.Cost = dayLogs.Sum(l => l.Quota);
                dailyDto.RequestCount = dayLogs.Count;
                dailyDto.TokenCount = dayLogs.Sum(l => l.PromptTokens + l.CompletionTokens);

                // 按模型分组
                var modelGroups = dayLogs
                    .GroupBy(l => l.ModelName)
                    .Select(modelGroup => new ModelUsageDto
                    {
                        ModelName = modelGroup.Key ?? "未知模型",
                        Cost = modelGroup.Sum(l => l.Quota),
                        RequestCount = modelGroup.Count(),
                        TokenCount = modelGroup.Sum(l => l.PromptTokens + l.CompletionTokens)
                    })
                    .ToList();

                dailyDto.ModelUsage.AddRange(modelGroups);
            }

            response.DailyUsage.Add(dailyDto);
        }

        // 按日期降序排序
        response.DailyUsage = response.DailyUsage.OrderByDescending(d => d.Date).ToList();

        // 获取实际出现的API接口和模型
        var actualApis = logs.Select(l => l.Url ?? "未知接口").Distinct().ToList();
        var models = logs.Select(l => l.ModelName ?? "未知模型").Distinct().ToList();
        if (models.Count == 0) models.Add("未知模型");

        // 确保列表中包含所有已知的API服务
        var allApiEndpoints = GetAllApiEndpoints(actualApis);

        // 获取服务请求列表，确保每天和每个API都有记录
        var serviceRequestsDict = logs
            .GroupBy(l => new
            {
                Date = l.CreatedAt.Date,
                ModelName = l.ModelName ?? "未知模型",
                Url = l.Url ?? "未知接口"
            })
            .ToDictionary(
                g => (g.Key.Date, g.Key.Url, g.Key.ModelName),
                g => new ServiceRequestDto
                {
                    Date = g.Key.Date,
                    ModelName = g.Key.ModelName,
                    ApiEndpoint = g.Key.Url,
                    RequestCount = g.Count(),
                    TokenCount = g.Sum(l => l.PromptTokens + l.CompletionTokens),
                    ImageCount = g.Sum(l => IsImageRequest(l.Url) ? 1 : 0),
                    Cost = g.Sum(l => l.Quota)
                }
            );

        var serviceRequests = new List<ServiceRequestDto>();

        // 为每一天、每个模型和每个API，生成服务请求记录
        foreach (var date in dateRange)
        {
            foreach (var model in models)
            {
                foreach (var api in allApiEndpoints)
                {
                    if (serviceRequestsDict.TryGetValue((date, api, model), out var request))
                    {
                        serviceRequests.Add(request);
                    }
                    else
                    {
                        serviceRequests.Add(new ServiceRequestDto
                        {
                            Date = date,
                            ModelName = model,
                            ApiEndpoint = api,
                            RequestCount = 0,
                            TokenCount = 0,
                            ImageCount = 0,
                            Cost = 0
                        });
                    }
                }
            }
        }

        response.ServiceRequests = serviceRequests.OrderByDescending(s => s.Date).ToList();

        return response;
    }

    /// <summary>
    /// 获取从开始日期到结束日期的每一天
    /// </summary>
    private List<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
    {
        var dates = new List<DateTime>();
        var currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            dates.Add(currentDate);
            currentDate = currentDate.AddDays(1);
        }

        return dates;
    }

    /// <summary>
    /// 判断是否为图片请求
    /// </summary>
    private bool IsImageRequest(string url)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        return url.Contains("/v1/images/generations") ||
               url.Contains("/v1/images/edits") ||
               url.Contains("/v1/images/variations");
    }

    /// <summary>
    /// 获取所有API端点，包括已知的服务类型和实际出现的URLs
    /// </summary>
    private List<string> GetAllApiEndpoints(List<string> actualApis)
    {
        var allApis = new HashSet<string>(actualApis);

        // 添加所有已知的API服务类型
        foreach (var knownApi in KnownApiServices.Values)
        {
            // 已知API可能是多个URL的代表，这里只添加主要URL
            if (!allApis.Any(api => api.Contains(knownApi)))
            {
                allApis.Add(knownApi);
            }
        }

        // 添加一个"未知接口"选项，以防有API没有URL
        if (!allApis.Contains("未知接口") && (actualApis.Count == 0 || actualApis.Contains("未知接口")))
        {
            allApis.Add("未知接口");
        }

        return allApis.ToList();
    }
}