using System.Collections.Concurrent;
using Thor.Core.DataAccess;

namespace Thor.Service.Service;

/// <summary>
/// 模型管理服务
/// </summary>
/// <param name="serviceProvider"></param>
public class ModelManagerService(IServiceProvider serviceProvider)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public static ConcurrentDictionary<string, ModelManager> PromptRate { get; private set; } = new();


    public static async ValueTask LoadingSettings(IThorContext context)
    {
        var models = await context.ModelManagers.Where(x => x.Enable).ToListAsync();

        PromptRate.Clear();

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
    }

    public async ValueTask DeleteAsync(Guid id)
    {
        await DbContext.ModelManagers
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        await LoadingSettings(DbContext);
    }

    public async ValueTask<PagingDto<ModelManager>> GetListAsync(string? model, int page, int pageSize, bool isPublic)
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

        var total = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.Enable)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagingDto<ModelManager>(total, data);
    }

    public async ValueTask EnableAsync(Guid id)
    {
        await DbContext.ModelManagers
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.Enable, y => !y.Enable));

        await LoadingSettings(DbContext);


    }
}