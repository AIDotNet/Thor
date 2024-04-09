using AIDotNet.Abstractions;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Model;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using TokenApi.Contract.Domain;

namespace AIDotNet.API.Service.Service;

public sealed class ChannelService(IServiceProvider serviceProvider, IMapper mapper)
    : ApplicationService(serviceProvider)
{
    public async Task CreateAsync(ChatChannelInput chatChannel)
    {
        var result = mapper.Map<ChatChannel>(chatChannel);
        result.Id = Guid.NewGuid().ToString();
        await DbContext.Channels.AddAsync(result);

        await DbContext.SaveChangesAsync();
    }

    public async Task<PagingDto<ChatChannel>> GetAsync(int page, int pageSize)
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


            return new PagingDto<ChatChannel>(total, result);
        }

        return new PagingDto<ChatChannel>(total, new List<ChatChannel>());
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
}