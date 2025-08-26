using System.Diagnostics;
using Thor.BuildingBlocks.Event;
using Thor.Domain.Chats;
using Thor.Service.Options;
using MiniExcelLibs;
using System.IO;
using Thor.Infrastructure;

namespace Thor.Service.Service;

public sealed class LoggerService(
    IServiceProvider serviceProvider,
    IEventBus<ChatLogger> eventBus,
    IEventBus<Tracing> tracingEventBus,
    IServiceCache serviceCache)
    : ApplicationService(serviceProvider)
{
    public async ValueTask CreateAsync(ChatLogger logger)
    {
        var tracing = TracingExtensions.GetCurrentRootTracing();

        if (string.IsNullOrEmpty(logger.Id))
        {
            logger.Id = Guid.NewGuid().ToString("N") + Random.Shared.Next(1000, 9999);
        }

        logger.CreatedAt = DateTime.Now;
        await eventBus.PublishAsync(logger);
        if (tracing != null)
        {
            tracing.ChatLoggerId = logger.Id;
            await tracingEventBus.PublishAsync(tracing);
        }
    }

    /// <summary>
    /// 创建消费日志
    /// </summary>
    /// <param name="content"></param>
    /// <param name="model"></param>
    /// <param name="promptTokens"></param>
    /// <param name="completionTokens"></param>
    /// <param name="quota"></param>
    /// <param name="tokenName"></param>
    /// <param name="userName"></param>
    /// <param name="userId"></param>
    /// <param name="channelId"></param>
    /// <param name="channelName"></param>
    /// <param name="ip"></param>
    /// <param name="userAgent"></param>
    /// <param name="stream">是否Stream请求</param>
    /// <param name="totalTime">请求总耗时</param>
    /// <param name="organizationId"></param>
    public async ValueTask CreateConsumeAsync(string url, string content, string model, int promptTokens,
        int completionTokens,
        int quota, string? tokenName, string? userName, string? userId, string? channelId, string? channelName,
        string ip, string userAgent, bool stream, int totalTime, string? organizationId = null,
        string? serviceId = null, string openAIProject = null,
        bool isSuccess = true, Dictionary<string, string>? meatdata = null
    )
    {
        meatdata ??= new Dictionary<string, string>();

        {
            using var consume =
                Activity.Current?.Source.StartActivity("创建消费日志");

            consume?.SetTag("Content", content);
            consume?.SetTag("model", model);

            if (ChatCoreOptions.FreeModel?.EnableFree == true)
            {
                var freeModel =
                    ChatCoreOptions.FreeModel.Items?.FirstOrDefault(x => x.Model.Contains(model));
                if (freeModel != null)
                {
                    string key = "FreeModal:" + userId;
                    var result = await serviceCache.GetAsync<int?>(key) ?? 0;
                    if (result < freeModel.Limit)
                    {
                        quota = 0;
                    }
                }
            }
        }

        var logger = new ChatLogger
        {
            Type = ThorChatLoggerType.Consume,
            Content = content,
            ModelName = model,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            Stream = stream,
            ServiceId = serviceId,
            OpenAIProject = openAIProject,
            Metadata = meatdata,
            IsSuccess = isSuccess,
            Url = url,
            TotalTime = totalTime,
            IP = ip,
            UserAgent = userAgent,
            Quota = quota,
            TokenName = tokenName,
            UserName = userName,
            UserId = userId,
            ChannelId = channelId,
            ChannelName = channelName,
            OrganizationId = organizationId
        };

        await CreateAsync(logger);
    }


    public async ValueTask CreateRechargeAsync(string content, int quota, string userId)
    {
        var logger = new ChatLogger
        {
            Type = ThorChatLoggerType.Recharge,
            Content = content,
            UserId = userId,
            Quota = quota,
            ModelName = string.Empty
        };
        await CreateAsync(logger);
    }

    public async ValueTask CreateSystemAsync(string content)
    {
        var logger = new ChatLogger
        {
            Type = ThorChatLoggerType.CreateUser,
            Content = content,
            ModelName = string.Empty
        };
        await CreateAsync(logger);
    }

    public async Task<long> ViewConsumptionAsync(ThorChatLoggerType? type,
        string? model,
        DateTime? startTime, DateTime? endTime, string? keyword, string? userId = null)
    {
        var query = LoggerDbContext.Loggers
            .AsNoTracking();

        if ((int)type == -1) type = null;

        if (type.HasValue) query = query.Where(x => x.Type == type);

        if (!string.IsNullOrWhiteSpace(model)) query = query.Where(x => x.ModelName == model);

        if (startTime.HasValue) query = query.Where(x => x.CreatedAt >= startTime);

        if (endTime.HasValue) query = query.Where(x => x.CreatedAt <= endTime);

        // 用户权限校验
        if (!UserContext.IsAdmin)
        {
            // 非管理员只能查看自己的日志
            query = query.Where(x => x.UserId == UserContext.CurrentUserId);
        }
        else if (!string.IsNullOrWhiteSpace(userId))
        {
            // 管理员可以查看指定用户的日志
            query = query.Where(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x =>
                x.UserName!.Contains(keyword) || x.Content.Contains(keyword) || x.TokenName.Contains(keyword) ||
                (!string.IsNullOrEmpty(x.ChannelName) && x.ChannelName.Contains(keyword)) ||
                x.ModelName.Contains(keyword)
            );

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .SumAsync(x => x.Quota);

        return result;
    }

    public async ValueTask<PagingDto<ChatLogger>> GetAsync(int page, int pageSize, ThorChatLoggerType? type,
        string? model,
        DateTime? startTime, DateTime? endTime, string? keyword, string? organizationId = null, string? userId = null)
    {
        var query = LoggerDbContext.Loggers
            .AsNoTracking();

        if ((int)type == -1) type = null;

        if (type.HasValue) query = query.Where(x => x.Type == type);

        if (!string.IsNullOrWhiteSpace(model)) query = query.Where(x => x.ModelName == model);

        if (startTime.HasValue) query = query.Where(x => x.CreatedAt >= startTime);

        if (endTime.HasValue) query = query.Where(x => x.CreatedAt <= endTime);

        // 用户权限校验
        if (!UserContext.IsAdmin)
        {
            // 非管理员只能查看自己的日志
            query = query.Where(x => x.UserId == UserContext.CurrentUserId);
        }
        else if (!string.IsNullOrWhiteSpace(userId))
        {
            // 管理员可以查看指定用户的日志
            query = query.Where(x => x.UserId == userId);
        }

        if (!string.IsNullOrEmpty(organizationId))
        {
            query = query.Where(x => x.OrganizationId == organizationId);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(x =>
                x.UserName!.Contains(keyword) || x.Content.Contains(keyword) || x.TokenName.Contains(keyword) ||
                (!string.IsNullOrEmpty(x.ChannelName) && x.ChannelName.Contains(keyword)) ||
                x.ModelName.Contains(keyword)
            );

        var total = await query.CountAsync();

        if (total <= 0) return new PagingDto<ChatLogger>(total, []);

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!UserContext.IsAdmin) result.ForEach(x => { x.ChannelName = null; });

        // 给所有的key脱敏,只显示前面3位和后面3位
        result.ForEach(x =>
        {
            if (!string.IsNullOrEmpty(x.TokenName))
            {
                x.TokenName = x.TokenName[..3] + "..." +
                              x.TokenName[^3..];
            }
        });

        return new PagingDto<ChatLogger>(total, result);
    }

    /// <summary>
    /// 导出日志
    /// </summary>
    public async Task ExportAsync(
        HttpContext context,
        ThorChatLoggerType? type,
        string? model,
        DateTime? startTime, DateTime? endTime, string? keyword, string? organizationId = null, string? userId = null)
    {
        var result = await GetAsync(1, int.MaxValue, type, model, startTime, endTime, keyword, organizationId, userId);

        // 使用MiniExcel导出
        if (result.Items.Any())
        {
            // 创建导出数据
            var exportData = result.Items.Select(log => new
            {
                日志类型 = log.Type.ToString(),
                模型名称 = log.ModelName,
                用户名 = log.UserName,
                用户ID = log.UserId,
                IP地址 = log.IP,
                内容 = log.Content,
                提示词Token = log.PromptTokens,
                完成Token = log.CompletionTokens,
                总消耗 = log.Quota,
                额度 = RenderHelper.RenderQuota(log.Quota, 2),
                响应时间 = log.TotalTime,
                是否成功 = log.IsSuccess ? "是" : "否",
                创建时间 = log.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            // 设置文件名称
            string fileName = $"{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // 设置响应头
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            var exportStream = new MemoryStream();
            await exportStream.SaveAsAsync(exportData);

            exportStream.Seek(0, SeekOrigin.Begin);

            await exportStream.CopyToAsync(context.Response.Body);
        }
        else
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("没有找到符合条件的数据");
        }
    }

    /// <summary>
    /// 模型热榜
    /// 统计最近15天的模型使用比例，返回模型名称和使用百分比
    /// 使用ModelStatisticsNumber
    /// </summary>
    /// <returns></returns>
    public async Task<List<object>> GetModelHotAsync()
    {
        return await serviceCache.GetOrCreateAsync("ModelHot", async () =>
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-15);
            var year = startDate.Year;
            var month = startDate.Month;
            var day = startDate.Day;

            var query = LoggerDbContext.ModelStatisticsNumbers
                .AsNoTracking()
                .Where(x =>
                    (x.Year > year) ||
                    (x.Year == year && x.Month > month) ||
                    (x.Year == year && x.Month == month && x.Day >= day)
                )
                .GroupBy(x => x.ModelName)
                .Select(g => new
                {
                    ModelName = g.Key,
                    TotalCount = g.Sum(x => x.Count)
                });

            var results = await query.ToListAsync();
            results = results.Where(x => !x.ModelName.Contains("text-embedding")).ToList();
            var totalUsage = results.Sum(x => x.TotalCount);

            return results
                .Select(x => new
                {
                    model = x.ModelName,
                    percentage = totalUsage > 0
                        ? Math.Round((double)x.TotalCount / totalUsage * 100, 2)
                        : 0
                })
                .OrderByDescending(x => x.percentage)
                .Cast<object>()
                .ToList();
        }, TimeSpan.FromHours(1)) ?? [];
    }
}