using System.Collections.Concurrent;
using Thor.BuildingBlocks.Event;
using Thor.Core.DataAccess;
using Thor.Service.Eto;

namespace Thor.Service.Service;

/// <summary>
/// 模型管理服务
/// </summary>
/// <param name="serviceProvider"></param>
public sealed class ModelManagerService(
    IServiceProvider serviceProvider,
    IEventBus<UpdateModelManagerCache> eventBus)
    : ApplicationService(serviceProvider)
{
    public static ConcurrentDictionary<string, ModelManager> PromptRate { get; } = new();


    public static async ValueTask LoadingSettings(IThorContext context)
    {
        var models = await context.ModelManagers.Where(x => x.Enable).ToListAsync();

        // PromptRate.Clear();

        foreach (var setting in models)
        {
            PromptRate[setting.Model] = setting;
        }
    }

    public async ValueTask CreateAsync(CreateModelManagerInput input)
    {
        if (string.IsNullOrEmpty(input.Model))
        {
            throw new("模型名称不能为空");
        }

        // 判断是否存在同名模型
        if (await DbContext.ModelManagers.AnyAsync(x => x.Model == input.Model))
        {
            throw new("已存在同名模型");
        }

        var entity = Mapper.Map<ModelManager>(input);

        await DbContext.ModelManagers.AddAsync(entity);

        await DbContext.SaveChangesAsync();

        await LoadingSettings(DbContext);

        await eventBus.PublishAsync(new UpdateModelManagerCache()
        {
            CreatedAt = DateTime.Now
        });
    }

    public async ValueTask UpdateAsync(UpdateModelManagerInput input)
    {
        if (await DbContext.ModelManagers.AnyAsync(x => x.Model == input.Model && x.Id != input.Id))
        {
            throw new("已存在同名模型");
        }

        var entity = await DbContext.ModelManagers.FindAsync(input.Id);

        if (entity == null)
        {
            throw new("模型不存在");
        }

        input.Enable = entity.Enable;

        Mapper.Map(input, entity);

        DbContext.ModelManagers.Update(entity);

        await DbContext.SaveChangesAsync();

        await LoadingSettings(DbContext);
        await eventBus.PublishAsync(new UpdateModelManagerCache()
        {
            CreatedAt = DateTime.Now
        });
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        await DbContext.ModelManagers
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        await LoadingSettings(DbContext);
        await eventBus.PublishAsync(new UpdateModelManagerCache()
        {
            CreatedAt = DateTime.Now
        });
    }

    public async ValueTask<PagingDto<ModelManager>> GetListAsync(string? model, int page, int pageSize, bool isPublic,
        string? type, string[]? tags)
    {
        var query = DbContext.ModelManagers.AsQueryable();

        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(x => x.Model.StartsWith(model) || x.Description.Contains(model));
        }

        if (isPublic)
        {
            query = query.Where(x => x.Enable);
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(x => x.Icon == type);
        }

        if (tags is { Length: > 0 })
        {
            // 如果需要过滤tag则先查询到内存
            var tagsList = await DbContext.ModelManagers
                .AsNoTracking()
                .ToListAsync();

            tagsList = tagsList
                .Where(x => x.Tags.Any(tags.Contains))
                .ToList();

            var total = tagsList.Count;

            tagsList = tagsList
                .Where(x => x.Enable)
                .OrderByDescending(x => x.Enable)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagingDto<ModelManager>(total, tagsList);
        }
        else
        {
            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Enable)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<ModelManager>(total, data);
        }
    }

    public async ValueTask EnableAsync(Guid id)
    {
        await DbContext.ModelManagers
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Enable, y => !y.Enable));

        await LoadingSettings(DbContext);

        await eventBus.PublishAsync(new UpdateModelManagerCache()
        {
            CreatedAt = DateTime.Now
        });
    }
}