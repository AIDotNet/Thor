using System.Collections.Concurrent;

namespace Thor.Service.Service;

/// <summary>
/// 模型管理服务
/// </summary>
/// <param name="serviceProvider"></param>
public class ModelManagerService(IServiceProvider serviceProvider)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public static ConcurrentDictionary<string, decimal> PromptRate { get; private set; } = new();
    public static ConcurrentDictionary<string, decimal> CompletionRate { get; private set; } = new();

    public static async ValueTask LoadingSettings(AIDotNetDbContext context)
    {
        var settings = await context.ModelManagers.Where(x => x.Enable).ToListAsync();

        PromptRate.Clear();
        CompletionRate.Clear();

        foreach (var setting in settings)
        {
            PromptRate[setting.Model] = setting.PromptRate;
            if (setting.CompletionRate is > 0)
            {
                CompletionRate[setting.Model] = setting.CompletionRate.Value;
            }
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