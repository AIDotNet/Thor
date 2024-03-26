using AIDotNet.Abstractions;
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
        var result = await DbContext.Channels
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var total = await DbContext.Channels.CountAsync();

        return new PagingDto<ChatChannel>(total, result);
    }

    public async Task<ResultDto<bool>> RemoveAsync(string id)
    {
        var result = await DbContext.Channels.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return ResultDto<bool>.CreateSuccess(result > 0);
    }

    public async Task<ResultDto<ChatChannel>> GetAsync(string id)
    {
        var chatChannel = await DbContext.Channels.FindAsync(id);
        if (chatChannel == null)
        {
            return ResultDto<ChatChannel>.CreateFail("渠道不存在");
        }

        return ResultDto<ChatChannel>.CreateSuccess(chatChannel);
    }

    public async Task<ResultDto<bool>> UpdateAsync(ChatChannel chatChannel)
    {
        var result = await DbContext.Channels.Where(x => x.Id == chatChannel.Id)
            .ExecuteUpdateAsync(item =>
                item.SetProperty(x => x.Type, chatChannel.Type)
                    .SetProperty(x => x.Name, chatChannel.Name)
                    .SetProperty(x => x.Address, chatChannel.Address)
                    .SetProperty(x => x.Key, chatChannel.Key)
                    .SetProperty(x => x.Other, chatChannel.Other)
                    .SetProperty(x => x.Models, chatChannel.Models));

        return ResultDto<bool>.CreateSuccess(result > 0);
    }

    /// <summary>
    /// 获取支持的模型服务
    /// </summary>
    /// <returns></returns>
    public ResultDto<Dictionary<string, string>> GetModelServices()
        => ResultDto<Dictionary<string, string>>.CreateSuccess(IADNChatCompletionService.ServiceNames);
}