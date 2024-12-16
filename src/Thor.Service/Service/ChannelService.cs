using MapsterMapper;
using OpenAI.Chat;
using System.Diagnostics;
using System.Threading.Channels;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Consts;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.AzureOpenAI;
using Thor.Claude;
using Thor.Core;
using Thor.Hunyuan;
using Thor.OpenAI;
using Thor.Service.Infrastructure;
using Thor.SparkDesk;

namespace Thor.Service.Service;

/// <summary>
/// 渠道管理
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="mapper"></param>
public sealed class ChannelService(IServiceProvider serviceProvider, IMapper mapper, IServiceCache cache)
    : ApplicationService(serviceProvider), IScopeDependency
{
    private const string CacheKey = "CacheKey:Channel";

    /// <summary>
    /// 获取渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    public async Task<ChatChannel[]> GetChannelsAsync()
    {
        return await cache.GetOrCreateAsync(CacheKey,
            async () => { return await DbContext.Channels.AsNoTracking().Where(x => !x.Disable).ToArrayAsync(); },
            isLock: false);
    }

    /// <summary>
    /// 获取包含指定模型名的渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    /// <param name="model">模型名</param>
    /// <returns></returns>
    public async ValueTask<IEnumerable<ChatChannel>> GetChannelsContainsModelAsync(string model)
    {
        return (await GetChannelsAsync()).Where(x => x.Models.Contains(model));
    }

    /// <summary>
    /// 创建渠道
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public async ValueTask CreateAsync(ChatChannelInput channel)
    {
        var result = mapper.Map<ChatChannel>(channel);
        result.Id = Guid.NewGuid().ToString();
        await DbContext.Channels.AddAsync(result);

        await DbContext.SaveChangesAsync();

        await cache.RemoveAsync(CacheKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async ValueTask<PagingDto<GetChatChannelDto>> GetAsync(int page, int pageSize)
    {
        var total = await DbContext.Channels.CountAsync();

        if (total > 0)
        {
            var result = await DbContext.Channels
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<GetChatChannelDto>(total, mapper.Map<List<GetChatChannelDto>>(result));
        }

        return new PagingDto<GetChatChannelDto>(total, new List<GetChatChannelDto>());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(string id)
    {
        var result = await DbContext.Channels.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        await cache.RemoveAsync(CacheKey);

        return result > 0;
    }

    public async ValueTask<ChatChannel> GetAsync(string id)
    {
        var chatChannel = await DbContext.Channels.FindAsync(id);
        if (chatChannel == null) throw new Exception("渠道不存在");

        return chatChannel;
    }

    public async ValueTask<bool> UpdateAsync(string id, ChatChannelInput chatChannel)
    {
        if (string.IsNullOrWhiteSpace(chatChannel.Key))
        {
            var result = await DbContext.Channels.Where(x => x.Id == id)
                .ExecuteUpdateAsync(item =>
                    item.SetProperty(x => x.Type, chatChannel.Type)
                        .SetProperty(x => x.Name, chatChannel.Name)
                        .SetProperty(x => x.Address, chatChannel.Address)
                        .SetProperty(x => x.Other, chatChannel.Other)
                        .SetProperty(x => x.Extension, chatChannel.Extension)
                        .SetProperty(x => x.Models, chatChannel.Models));
            await cache.RemoveAsync(CacheKey);
            return result > 0;
        }
        else
        {
            var result = await DbContext.Channels.Where(x => x.Id == id)
                .ExecuteUpdateAsync(item =>
                    item.SetProperty(x => x.Type, chatChannel.Type)
                        .SetProperty(x => x.Name, chatChannel.Name)
                        .SetProperty(x => x.Key, chatChannel.Key)
                        .SetProperty(x => x.Address, chatChannel.Address)
                        .SetProperty(x => x.Extension, chatChannel.Extension)
                        .SetProperty(x => x.Other, chatChannel.Other)
                        .SetProperty(x => x.Models, chatChannel.Models));
            await cache.RemoveAsync(CacheKey);

            return result > 0;
        }
    }

    public async ValueTask UpdateOrderAsync(string id, int order)
    {
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Order, order));

        await cache.RemoveAsync(CacheKey);
    }

    public async ValueTask ControlAutomaticallyAsync(string id)
    {
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ControlAutomatically, a => !a.ControlAutomatically));
    }

    public async ValueTask DisableAsync(string id)
    {
        // 更新状态
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Disable, a => !a.Disable));

        await cache.RemoveAsync(CacheKey);
    }

    /// <summary>
    /// 测试渠道接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ChannelException"></exception>
    public async ValueTask<(bool, int)> TestChannelAsync(string id)
    {
        var channel = await DbContext.Channels.FindAsync(id);

        if (channel == null)
        {
            throw new Exception("渠道不存在");
        }

        // 获取渠道指定的实现类型的服务
        var chatCompletionsService = GetKeyedService<IThorChatCompletionsService>(channel.Type);

        if (chatCompletionsService == null)
        {
            throw new Exception("渠道服务不存在");
        }

        var chatRequest = new ThorChatCompletionsRequest()
        {
            TopP = 0.7f,
            Temperature = 0.95f,
            MaxTokens = 100,
            Messages = [ThorChatMessage.CreateUserMessage("hello")]
        };

        var platformOptions = new ThorPlatformOptions
        {
            Address = channel.Address,
            ApiKey = channel.Key,
            Other = channel.Other
        };

        if (channel.Type is OpenAIPlatformOptions.PlatformCode or AzureOpenAIPlatformOptions.PlatformCode)
        {
            // 如果没gpt3.5则搜索是否存在gpt4o模型
            chatRequest.Model = channel.Models?.FirstOrDefault(x =>
                x.StartsWith("gpt-4o-mini", StringComparison.OrdinalIgnoreCase)) ?? channel.Models!.First();

            if (string.IsNullOrEmpty(chatRequest.Model))
            {
                // 获取渠道是否支持gpt-3.5-turbo
                chatRequest.Model = channel.Models.Order()
                    .FirstOrDefault(x => x.StartsWith("gpt-", StringComparison.OrdinalIgnoreCase));
            }
        }
        else if (channel.Type == ClaudiaPlatformOptions.PlatformCode)
        {
            chatRequest.Model =
                channel.Models.FirstOrDefault(x => x.StartsWith("claude", StringComparison.OrdinalIgnoreCase));
        }
        else if (channel.Type == SparkDeskPlatformOptions.PlatformCode)
        {
            chatRequest.Model = channel.Models.FirstOrDefault(x =>
                x.StartsWith("general", StringComparison.OrdinalIgnoreCase) ||
                x.StartsWith("SparkDesk", StringComparison.OrdinalIgnoreCase));
        }
        else if (channel.Type == HunyuanPlatformOptions.PlatformCode)
        {
            chatRequest.Model =
                channel.Models.FirstOrDefault(x => !x.Contains("embedding", StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            chatRequest.Model = channel.Models.First();
        }

        if (string.IsNullOrWhiteSpace(chatRequest.Model))
        {
            chatRequest.Model = channel.Models!.First();
        }

        var token = new CancellationTokenSource();

        var sw = Stopwatch.StartNew();

        var response = await chatCompletionsService.ChatCompletionsAsync(chatRequest,
            platformOptions,
            token.Token);

        sw.Stop();

        // 更新渠道测试响应时间
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResponseTime, sw.ElapsedMilliseconds),
                cancellationToken: token.Token);

        if (!string.IsNullOrWhiteSpace(response.Error?.Message))
        {
            throw new ChannelException(response.Error?.Message);
        }

        if (response.Choices?.Count == 0)
        {
            throw new ChannelException("渠道返回数据为空");
        }

        return (response.Choices?.Count > 0, (int)sw.ElapsedMilliseconds);
    }
}