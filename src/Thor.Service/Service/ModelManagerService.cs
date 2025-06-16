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
        string? type, string[]? tags, bool? enabled = null)
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

        // 添加启用状态筛选
        if (enabled.HasValue)
        {
            query = query.Where(x => x.Enable == enabled.Value);
        }

        if (!string.IsNullOrEmpty(type))
        {
            // 支持按模型类型筛选
            if (type.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                // 不添加过滤条件，显示所有类型
            }
            else
            {
                query = query.Where(x => x.Type == type);
            }
        }

        // 如果是通过图标进行筛选（兼容旧代码）
        if (!string.IsNullOrEmpty(type) && !type.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            // 检查是否是按图标筛选
            var isIconFilter = await DbContext.ModelManagers
                .AnyAsync(x => x.Icon == type);

            if (isIconFilter)
            {
                query = query.Where(x => x.Icon == type);
            }
        }

        if (tags != null && tags.Length > 0)
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
                .Where(x => enabled == null || x.Enable == enabled.Value)
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

    /// <summary>
    /// 获取模型统计信息
    /// </summary>
    /// <returns></returns>
    public async ValueTask<Dictionary<string, int>> GetModelStatsAsync()
    {
        var models = await DbContext.ModelManagers.ToListAsync();

        var stats = new Dictionary<string, int>
        {
            ["total"] = models.Count,
            ["enabled"] = models.Count(x => x.Enable),
            ["disabled"] = models.Count(x => !x.Enable),
            ["chat"] = models.Count(x => x.Type == "chat"),
            ["image"] = models.Count(x => x.Type == "image"),
            ["audio"] = models.Count(x => x.Type == "audio"),
            ["embedding"] = models.Count(x => x.Type == "embedding"),
            ["stt"] = models.Count(x => x.Type == "stt"),
            ["tts"] = models.Count(x => x.Type == "tts")
        };

        return stats;
    }

    /// <summary>
    /// 获取所有可用的模型类型
    /// </summary>
    /// <returns></returns>
    public async ValueTask<List<string>> GetModelTypesAsync()
    {
        var types = await DbContext.ModelManagers
            .Where(x => !string.IsNullOrEmpty(x.Type))
            .Select(x => x.Type)
            .Distinct()
            .ToListAsync();

        return types;
    }

    /// <summary>
    /// 获取所有可用的标签
    /// </summary>
    /// <returns></returns>
    public async ValueTask<List<string>> GetAllTagsAsync()
    {
        var models = await DbContext.ModelManagers
            .ToListAsync();

        var allTags = models
            .SelectMany(x => x.Tags)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        return allTags;
    }

    /// <summary>
    /// 获取模型库元数据信息（包含tags、types、providers、icons等）
    /// </summary>
    /// <returns></returns>
    public async ValueTask<object> GetMetadataAsync()
    {
        var models = await DbContext.ModelManagers
            .AsNoTracking()
            .ToListAsync();

        // 获取所有标签
        var allTags = models
            .SelectMany(x => x.Tags)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // 获取所有模型类型及其数量
        var modelTypes = models
            .Where(x => !string.IsNullOrEmpty(x.Type))
            .Select(x => x.Type)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        // 计算每个模型类型的数量
        var modelTypeCounts = models
            .Where(x => !string.IsNullOrEmpty(x.Type))
            .GroupBy(x => x.Type)
            .Select(g => new
            {
                type = g.Key,
                count = g.Count()
            })
            .OrderBy(x => x.type)
            .ToList();

        // 获取所有供应商信息（基于Icon字段）
        var providers = new Dictionary<string, string>();
        var icons = models
            .Where(x => !string.IsNullOrEmpty(x.Icon))
            .Select(x => x.Icon)
            .Distinct()
            .ToList();

        foreach (var icon in icons)
        {
            switch (icon)
            {
                case "OpenAI":
                    providers.Add("OpenAI", "OpenAI");
                    break;
                case "DeepSeek":
                    providers.Add("DeepSeek", "深度求索");
                    break;
                case "Claude":
                    providers.Add("Claude", "Claude");
                    break;
                case "ChatGLM":
                    providers.Add("ChatGLM", "ChatGLM");
                    break;
                case "SiliconCloud":
                    providers.Add("SiliconCloud", "硅基流动");
                    break;
                case "Moonshot":
                    providers.Add("Moonshot", "Moonshot");
                    break;
                case "Mistral":
                    providers.Add("Mistral", "Mistral");
                    break;
                case "Gemini":
                    providers.Add("Gemini", "Gemini");
                    break;
                case "ErnieBot":
                    providers.Add("ErnieBot", "文心一言");
                    break;
                case "SparkDesk":
                    providers.Add("SparkDesk", "讯飞星火");
                    break;
                case "Hunyuan":
                    providers.Add("Hunyuan", "腾讯混元");
                    break;
                case "MiniMax":
                    providers.Add("MiniMax", "MiniMax");
                    break;
                case "MetaGLM":
                    providers.Add("MetaGLM", "智谱清言");
                    break;
                case "GiteeAI":
                    providers.Add("GiteeAI", "Gitee AI");
                    break;
                case "VolCenGine":
                    providers.Add("VolCenGine", "火山引擎");
                    break;
                case "Qiansail":
                    providers.Add("Qiansail", "千帆大模型");
                    break;
                case "Ollama":
                    providers.Add("Ollama", "Ollama");
                    break;
                case "AWSClaude":
                    providers.Add("AWSClaude", "AWS Claude");
                    break;
                case "GCPClaude":
                    providers.Add("GCPClaude", "GCP Claude");
                    break;
                case "AzureOpenAI":
                    providers.Add("AzureOpenAI", "Azure OpenAI");
                    break;
                default:
                    providers.Add(icon, icon);
                    break;
            }
        }

        // 计算每个供应商的模型数量
        var providerCounts = models
            .Where(x => !string.IsNullOrEmpty(x.Icon))
            .GroupBy(x => x.Icon)
            .Select(g => new
            {
                provider = g.Key,
                count = g.Count()
            })
            .OrderBy(x => x.provider)
            .ToList();

        // 构建图标映射（使用Icon字段作为key和value）
        var iconMapping = icons.ToDictionary(x => x, x => x);

        return new
        {
            tags = allTags,
            providers = providers,
            icons = iconMapping,
            modelTypes = modelTypes,
            modelTypeCounts = modelTypeCounts,
            providerCounts = providerCounts
        };
    }
}