using AIDotNet.Abstractions;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public sealed class LoggerService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    public async ValueTask CreateAsync(ChatLogger logger)
    {
        logger.Id = Guid.NewGuid().ToString("N");
        await LoggerDbContext.Loggers.AddAsync(logger);
    }

    public async ValueTask CreateConsumeAsync(string content, string model, int promptTokens, int completionTokens,
        int quota, string? tokenName, string? userName, string? userId, string? channelId, string? channelName)
    {
        var logger = new ChatLogger
        {
            Type = ChatLoggerType.Consume,
            Content = content,
            ModelName = model,
            PromptTokens = promptTokens,
            CompletionTokens = completionTokens,
            Quota = quota,
            TokenName = tokenName,
            UserName = userName,
            UserId = userId,
            ChannelId = channelId,
            ChannelName = channelName
        };

        await CreateAsync(logger);
    }

    public async ValueTask CreateRechargeAsync(string content, int quota, string userId)
    {
        var logger = new ChatLogger
        {
            Type = ChatLoggerType.Recharge,
            Content = content,
            UserId = userId,
            Quota = quota,
            ModelName = string.Empty,
        };
        await CreateAsync(logger);
    }

    public async ValueTask CreateSystemAsync(string content)
    {
        var logger = new ChatLogger
        {
            Type = ChatLoggerType.System,
            Content = content,
            ModelName = string.Empty,
        };
        await CreateAsync(logger);
    }


    public async ValueTask<PagingDto<ChatLogger>> GetAsync(int page, int pageSize, ChatLoggerType? type, string? model,
        DateTime? startTime, DateTime? endTime, string? keyword)
    {
        var query = LoggerDbContext.Loggers
            .AsNoTracking();

        if ((int)type == -1)
        {
            type = null;
        }

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(model))
        {
            query = query.Where(x => x.ModelName == model);
        }

        if (startTime.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= startTime);
        }

        if (endTime.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= endTime);
        }

        if (!UserContext.IsAdmin)
        {
            query = query.Where(x => x.UserId == UserContext.CurrentUserId);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x =>
                x.UserName!.Contains(keyword) || x.Content.Contains(keyword) || x.TokenName.Contains(keyword) ||
                (!string.IsNullOrEmpty(x.ChannelName) && x.ChannelName.Contains(keyword)) ||
                x.ModelName.Contains(keyword)
            );
        }

        var total = await query.CountAsync();

        if (total <= 0) return new PagingDto<ChatLogger>(total, []);

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!UserContext.IsAdmin)
        {
            result.ForEach(x =>
            {
                x.ChannelName = null;
            });
        }

        return new PagingDto<ChatLogger>(total, result);
    }
}