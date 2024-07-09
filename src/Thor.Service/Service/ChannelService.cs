using System.Diagnostics;
using Thor.Claudia;
using Thor.Hunyuan;
using Thor.OpenAI;
using Thor.SparkDesk;
using MapsterMapper;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Moonshot;

namespace Thor.Service.Service;

public sealed class ChannelService(IServiceProvider serviceProvider, IMapper mapper)
    : ApplicationService(serviceProvider)
{
    /// <summary>
    ///     获取渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    public async ValueTask<List<ChatChannel>> GetChannelsAsync()
    {
        return await DbContext.Channels.AsNoTracking().Where(x => !x.Disable).ToListAsync();
    }

    public async ValueTask CreateAsync(ChatChannelInput chatChannel)
    {
        var result = mapper.Map<ChatChannel>(chatChannel);
        result.Id = Guid.NewGuid().ToString();
        await DbContext.Channels.AddAsync(result);

        await DbContext.SaveChangesAsync();
    }

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

    public async Task<bool> RemoveAsync(string id)
    {
        var result = await DbContext.Channels.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

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

            return result > 0;
        }
    }

    public async ValueTask UpdateOrderAsync(string id, int order)
    {
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Order, order));
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
    }

    public async ValueTask<(bool, int)> TestChannelAsync(string id)
    {
        var channel = await DbContext.Channels.FindAsync(id);

        if (channel == null) throw new Exception("渠道不存在");

        // 获取渠道指定的实现类型的服务
        var openService = GetKeyedService<IApiChatCompletionService>(channel.Type);

        if (openService == null) throw new Exception("渠道服务不存在");

        var chatHistory = new ChatCompletionCreateRequest()
        {
            TopP=0.7f,
            Temperature=0.95f,
        };

        var setting = new ChatOptions
        {
            Address = channel.Address,
            ApiKey = channel.Key,
            Other = channel.Other
        };

        if (channel.Type == OpenAIPlatformOptions.PlatformCode)
        {
            // 获取渠道是否支持gpt-3.5-turbo
            chatHistory.Model = channel.Models.Order()
                .FirstOrDefault(x => x.StartsWith("gpt-", StringComparison.OrdinalIgnoreCase));

            chatHistory.Messages = new List<ChatMessage>
            {
                new()
                {
                    Content = "Return 1",
                    Role = "user"
                }
            };
        }
        else if (channel.Type == ClaudiaPlatformOptions.PlatformCode)
        {
            chatHistory.Model =
                channel.Models.FirstOrDefault(x => x.StartsWith("claude", StringComparison.OrdinalIgnoreCase));
            chatHistory.Messages = new List<ChatMessage>
            {
                new()
                {
                    Content = "Return 1",
                    Role = "user"
                }
            };
        }
        else if (channel.Type == SparkDeskPlatformOptions.PlatformCode)
        {
            chatHistory.Model = channel.Models.FirstOrDefault(x =>
                x.StartsWith("genera", StringComparison.OrdinalIgnoreCase) ||
                x.StartsWith("SparkDesk", StringComparison.OrdinalIgnoreCase));
            chatHistory.Messages = new List<ChatMessage>
            {
                new()
                {
                    Content = "回复ok",
                    Role = "user"
                }
            };
        }
        else if (channel.Type == HunyuanPlatformOptions.PlatformCode)
        {
            chatHistory.Model =
                channel.Models.FirstOrDefault(x => !x.Contains("embedding", StringComparison.OrdinalIgnoreCase));
            chatHistory.Messages = new List<ChatMessage>
            {
                new()
                {
                    Content = "回复ok",
                    Role = "user"
                }
            };
        }
        else
        {
            chatHistory.Model = channel.Models.FirstOrDefault();
            chatHistory.Messages = new List<ChatMessage>
            {
                new()
                {
                    Content = "Return 1",
                    Role = "user"
                }
            };
        }


        if (string.IsNullOrWhiteSpace(chatHistory.Model))
        {
            chatHistory.Model = channel.Models.FirstOrDefault();
        }

        // 写一个10s的超时
        var token = new CancellationTokenSource();
        // token.CancelAfter(20000);

        var sw = Stopwatch.StartNew();
        var response = await openService.CompleteChatAsync(chatHistory, setting, token.Token);
        sw.Stop();

        // 更新ResponseTime
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResponseTime, sw.ElapsedMilliseconds));

        if (!string.IsNullOrWhiteSpace(response.Error?.Message))
        {
            throw new ChannelException(response.Error?.Message);
        }

        return (response.Choices?.Count > 0,
            (int)sw.ElapsedMilliseconds);
    }
}