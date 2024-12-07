using System.Diagnostics;
using Thor.BuildingBlocks.Event;
using Thor.Service.Model;
using Thor.Service.Options;

namespace Thor.Service.Service;

public sealed class LoggerService(
    IServiceProvider serviceProvider,
    IEventBus<ChatLogger> eventBus,
    IServiceCache serviceCache)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public async ValueTask CreateAsync(ChatLogger logger)
    {
        logger.CreatedAt = DateTime.Now;
        await eventBus.PublishAsync(logger);
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
    public async ValueTask CreateConsumeAsync(string content, string model, int promptTokens, int completionTokens,
        int quota, string? tokenName, string? userName, string? userId, string? channelId, string? channelName,
        string ip, string userAgent, bool stream, int totalTime, string? organizationId = null)
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

        var logger = new ChatLogger
        {
            Type = ThorChatLoggerType.Consume,
            Content = content,
            ModelName = model,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            Stream = stream,
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
        DateTime? startTime, DateTime? endTime, string? keyword)
    {
        var query = LoggerDbContext.Loggers
            .AsNoTracking();

        if ((int)type == -1) type = null;

        if (type.HasValue) query = query.Where(x => x.Type == type);

        if (!string.IsNullOrWhiteSpace(model)) query = query.Where(x => x.ModelName == model);

        if (startTime.HasValue) query = query.Where(x => x.CreatedAt >= startTime);

        if (endTime.HasValue) query = query.Where(x => x.CreatedAt <= endTime);

        if (!UserContext.IsAdmin) query = query.Where(x => x.UserId == UserContext.CurrentUserId);

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
        DateTime? startTime, DateTime? endTime, string? keyword,string? organizationId = null)
    {
        var query = LoggerDbContext.Loggers
            .AsNoTracking();

        if ((int)type == -1) type = null;

        if (type.HasValue) query = query.Where(x => x.Type == type);

        if (!string.IsNullOrWhiteSpace(model)) query = query.Where(x => x.ModelName == model);

        if (startTime.HasValue) query = query.Where(x => x.CreatedAt >= startTime);

        if (endTime.HasValue) query = query.Where(x => x.CreatedAt <= endTime);

        if (!UserContext.IsAdmin) query = query.Where(x => x.UserId == UserContext.CurrentUserId);

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

        return new PagingDto<ChatLogger>(total, result);
    }
}