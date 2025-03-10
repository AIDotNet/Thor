using System.Diagnostics;
using Thor.Abstractions.Exceptions;

namespace Thor.Service.Service;

/// <summary>
/// 模型映射管理
/// </summary>
/// <param name="serviceProvider"></param>
/// <param name="serviceCache"></param>
public sealed class ModelMapService(IServiceProvider serviceProvider, IServiceCache serviceCache)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public async ValueTask<string> ModelMap(string model)
    {
        var models =
            await serviceCache.GetOrCreateAsync("ModelMap", async () => await DbContext.ModelMaps.ToListAsync());

        using var modelMap =
            Activity.Current?.Source.StartActivity("模型映射转换");
        var modelItems = models?.FirstOrDefault(x => x.ModelId == model);
        
        if(modelItems == null)
        {
            return model;
        }
        
        var total = modelItems.ModelMapItems.Sum(x => x.Order);

        var value = Convert.ToInt32(Random.Shared.NextDouble() * total);

        foreach (var chatChannel in modelItems.ModelMapItems)
        {
            value -= chatChannel.Order;
            if (value <= 0)
            {
                modelMap?.SetTag("Model", chatChannel.ModelId);
                return chatChannel.ModelId;
            }
        }

        modelMap?.SetTag("Model", modelItems.ModelMapItems.LastOrDefault()?.ModelId ?? model);

        return modelItems.ModelMapItems?.LastOrDefault()?.ModelId ?? model;
    }

    /// <summary>
    /// 创建模型映射
    /// </summary>
    public async Task CreateAsync(ModelMap modelMap)
    {
        // 判断是否已经存在模型映射
        if (await DbContext.ModelMaps.AnyAsync(x => x.ModelId == modelMap.ModelId))
        {
            throw new BusinessException("模型映射已存在", "400");
        }

        await DbContext.ModelMaps.AddAsync(modelMap);

        await DbContext.SaveChangesAsync();

        // 清除缓存
        await serviceCache.RemoveAsync("ModelMap");
    }

    /// <summary>
    /// 更新模型映射
    /// </summary>
    public async Task UpdateAsync(ModelMap modelMap)
    {
        var entity = await DbContext.ModelMaps.FirstOrDefaultAsync(x => x.ModelId == modelMap.ModelId);

        if (entity == null)
        {
            throw new BusinessException("模型映射不存在", "400");
        }

        await DbContext.ModelMaps.Where(x => x.ModelId == modelMap.ModelId)
            .ExecuteUpdateAsync(a => a.SetProperty(x => x.Group, modelMap.Group)
                .SetProperty(a => a.ModelMapItems, modelMap.ModelMapItems)
                .SetProperty(a => a.Modifier, UserContext.CurrentUserId)
                .SetProperty(a => a.UpdatedAt, DateTime.Now));
    }

    /// <summary>
    /// 删除模型映射
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        await DbContext.ModelMaps.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        // 清除缓存
        await serviceCache.RemoveAsync("ModelMap");
    }

    /// <summary>
    /// 获取模型映射列表
    /// </summary>
    public async Task<List<ModelMap>> GetListAsync()
    {
        var modelMaps = await DbContext.ModelMaps.ToListAsync();

        return modelMaps;
    }
}