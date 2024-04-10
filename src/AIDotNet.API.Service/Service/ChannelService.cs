using System.Diagnostics;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.OpenAI;
using AIDotNet.SparkDesk;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.API.Service.Service;

public sealed class ChannelService(IServiceProvider serviceProvider, IMapper mapper, IMemoryCache cache)
    : ApplicationService(serviceProvider)
{
    /// <summary>
    /// 获取渠道列表 如果缓存中有则从缓存中获取
    /// </summary>
    public async ValueTask<List<ChatChannel>> GetChannelsAsync()
    {
#if DEBUG
        return await DbContext.Channels.AsNoTracking().Where(x => !x.Disable).ToListAsync();
#endif

        var channels = await cache.GetOrCreateAsync("channels", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return await DbContext.Channels.AsNoTracking().Where(x => !x.Disable).ToListAsync();
        });

        return channels;
    }

    public async Task CreateAsync(ChatChannelInput chatChannel)
    {
        var result = mapper.Map<ChatChannel>(chatChannel);
        result.Id = Guid.NewGuid().ToString();
        await DbContext.Channels.AddAsync(result);

        await DbContext.SaveChangesAsync();
    }

    public async Task<PagingDto<GetChatChannelDto>> GetAsync(int page, int pageSize)
    {
        var total = await DbContext.Channels.CountAsync();

        if (total > 0)
        {
            var result = await DbContext.Channels
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
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

    public async Task<ChatChannel> GetAsync(string id)
    {
        var chatChannel = await DbContext.Channels.FindAsync(id);
        if (chatChannel == null)
        {
            throw new Exception("渠道不存在");
        }

        return chatChannel;
    }

    public async Task<bool> UpdateAsync(string id, ChatChannelInput chatChannel)
    {
        var result = await DbContext.Channels.Where(x => x.Id == id)
            .ExecuteUpdateAsync(item =>
                item.SetProperty(x => x.Type, chatChannel.Type)
                    .SetProperty(x => x.Name, chatChannel.Name)
                    .SetProperty(x => x.Address, chatChannel.Address)
                    .SetProperty(x => x.Key, chatChannel.Key)
                    .SetProperty(x => x.Other, chatChannel.Other)
                    .SetProperty(x => x.Models, chatChannel.Models));

        return result > 0;
    }

    public async Task DisableAsync(string id)
    {
        // 更新状态
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Disable, a => !a.Disable));
    }

    public async Task<(bool, int)> TestChannelAsync(string id)
    {
        var channel = await DbContext.Channels.FindAsync(id);

        if (channel == null)
        {
            throw new Exception("渠道不存在");
        }

        // 获取渠道指定的实现类型的服务
        var openService = GetKeyedService<IChatCompletionService>(channel.Type);

        if (openService == null)
        {
            throw new Exception("渠道服务不存在");
        }

        var chatHistory = new OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput>();
        chatHistory.Messages.Add(new OpenAIChatCompletionRequestInput()
        {
            Content = "Return 1",
            Role = "user"
        });

        var setting = new ChatOptions()
        {
            Address = channel.Address,
            Key = channel.Key
        };

        switch (channel.Type)
        {
            case OpenAIServiceOptions.ServiceName:
                chatHistory.Model = "gpt-3.5-turbo";
                break;
            case SparkDeskOptions.ServiceName:
                chatHistory.Model = "SparkDesk-v3.5";
                break;
            default:
                chatHistory.Model = "gpt-3.5-turbo";
                break;
        }

        var sw = Stopwatch.StartNew();
        var response = await openService.CompleteChatAsync(chatHistory, setting);
        sw.Stop();

        // 更新ResponseTime
        await DbContext.Channels
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.ResponseTime, sw.ElapsedMilliseconds));

        await DbContext.SaveChangesAsync();

        return (response.Choices.Length > 0, (int)sw.ElapsedMilliseconds);
    }
}