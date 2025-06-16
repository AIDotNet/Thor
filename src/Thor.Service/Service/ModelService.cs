using Thor.Abstractions.Dtos;
using Thor.Core.DataAccess;
using Thor.Service.Model;

namespace Thor.Service.Service;

public static class ModelService
{
    /// <summary>
    /// 获取平台名列表
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> GetPlatformNames()
    {
        return ThorGlobal.PlatformNames;
    }

    public static string[] GetModels()
    {
        return ModelManagerService.PromptRate.Select(x => x.Key).ToArray();
    }

    public static async ValueTask<List<UseModelDto>> GetUseModels(HttpContext context)
    {
        var cache = context.RequestServices.GetRequiredService<IServiceCache>();

        var model = await cache.GetAsync<List<UseModelDto>>("UseModels").ConfigureAwait(false);
        if (model != null) return model;

        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();
        var loggerDbContext = context.RequestServices.GetRequiredService<ILoggerDbContext>();
        // 获取模型
        var channels = await dbContext.Channels.Where(x => x.Disable == false).ToListAsync();

        var value = channels.SelectMany(x => x.Models).Distinct()
            .Select(x => new UseModelDto
            {
                Model = x
            }).ToList();

        var now = DateTime.Now.AddDays(7);

        foreach (var item in value)
        {
            var count = await loggerDbContext.Loggers
                .Where(x => x.CreatedAt > now && x.ModelName == item.Model && x.Type == ThorChatLoggerType.Consume)
                .CountAsync();

            item.Count = count;
        }

        value = value.OrderByDescending(x => x.Count).ToList();

        // 修改前三模型
        for (var i = 0; i < value.Count; i++)
        {
            if (i == 3) break;

            if (value[i].Count > 100) value[i].Hot = true;
        }

        return value;
    }

    public static async Task<ModelsListDto> GetAsync(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();


        var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();


        List<ModelManager> models;
        // 如果是sk-开头的token，说明是自定义的token
        if (token.StartsWith("sk-"))
        {
            // 获取tokens
            var tokens = await dbContext.Tokens
                .Where(x => x.Key == token)
                .FirstOrDefaultAsync();


            if (tokens == null)
            {
                context.Response.StatusCode = 401;
                throw new UnauthorizedAccessException("Token不存在");
            }

            // 获取这个key的分组支持的渠道 
            var tokenGroups = tokens.Groups;

            var supportedModelsList = await dbContext.Channels
                .Where(x => x.Disable == false)
                .ToListAsync();

            var supportedModels = supportedModelsList
                .Where(x => tokenGroups.Any(a => x.Groups.Contains(a)))
                .SelectMany(x => x.Models).Distinct();

            models = await dbContext.ModelManagers
                .Where(x => x.Enable && supportedModels.Contains(x.Model))
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }
        else
        {
            models = await dbContext.ModelManagers
                .OrderBy(x => x.CreatedAt)
                .Where(x => x.Enable)
                .ToListAsync();
        }

        var modelsListDto = new ModelsListDto();

        foreach (var model in models)
        {
            modelsListDto.Data.Add(new ModelsDataDto()
            {
                Created = model.CreatedAt.ToUnixTimeSeconds(),
                Id = model.Model,
                @object = "model",
                OwnedBy = model.Icon?.ToLower() ?? "openai",
                Type = model.Type,
            });
        }

        return modelsListDto;
    }

    public static async Task<ModelInfoDto> GetModelInfoAsync(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();

        var models = await dbContext.ModelManagers
            .AsNoTracking()
            .Where(x => x.Enable && !string.IsNullOrEmpty(x.Type))
            .ToArrayAsync();

        var types = models
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        var modelInfo = new ModelInfoDto();

        foreach (var type in types)
        {
            modelInfo.ModelTypeCounts.Add(new ModelTypeCountDto()
            {
                Type = type,
                Count = await dbContext.ModelManagers.CountAsync(x => x.Type == type)
            });
        }

        // 根据type分组
        var modelType = models
            .GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.ToList());

        foreach (var type in modelType)
        {
            modelInfo.ModelTypes.Add(new ModelTypeDto()
            {
                Type = type.Key,
                Models = type.Value.Select(x => x.Model).ToArray()
            });
        }

        return modelInfo;
    }

    /// <summary>
    /// 获取所有渠道
    /// </summary>
    /// <returns></returns>
    public static async Task<Dictionary<string, string?>> GetProviderAsync(IThorContext context)
    {
        var channels = await context.ModelManagers.Select(x => x.Icon).Distinct().ToListAsync();

        var result = new Dictionary<string, string?>();
        foreach (var channel in channels.Where(x => !string.IsNullOrEmpty(x)))
        {
            switch (channel)
            {
                case "OpenAI":
                    result.Add("OpenAI", "OpenAI");
                    break;
                case "DeepSeek":
                    result.Add("DeepSeek", "深度求索");
                    break;
                case "Claude":
                    result.Add("Claude", "Claude");
                    break;
                case "ChatGLM":
                    result.Add("ChatGLM", "ChatGLM");
                    break;
                case "SiliconCloud":
                    result.Add("SiliconCloud", "硅基流动");
                    break;
                case "Moonshot":
                    result.Add("Moonshot", "Moonshot");
                    break;
                case "Mistral":
                    result.Add("Mistral", "Mistral");
                    break;
                default:
                    result.Add(channel, channel);
                    break;
            }
        }

        result.Add("其他", null);
        return result;
    }

    /// <summary>
    /// 获取模型库列表（用于模型库页面）
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <param name="model">模型名称搜索</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">页面大小</param>
    /// <param name="type">供应商类型</param>
    /// <param name="modelType">模型类型</param>
    /// <param name="tags">标签</param>
    /// <returns></returns>
    public static async Task<PagingDto<ModelManager>> GetModelLibraryAsync(HttpContext context, 
        string? model = null, int page = 1, int pageSize = 20, 
        string? type = null, string? modelType = null, string[]? tags = null)
    {
        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();
        
        var query = dbContext.ModelManagers.Where(x => x.Enable).AsQueryable();

        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(x => x.Model.StartsWith(model) || x.Description.Contains(model));
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(x => x.Icon == type);
        }

        if (!string.IsNullOrEmpty(modelType))
        {
            query = query.Where(x => x.Type.ToLower() == modelType.ToLower());
        }

        if (tags != null && tags.Length > 0)
        {
            // 如果需要过滤tag则先查询到内存
            var tagsList = await dbContext.ModelManagers
                .AsNoTracking()
                .Where(x => x.Enable)
                .ToListAsync();

            tagsList = tagsList
                .Where(x => x.Tags.Any(tags.Contains))
                .ToList();

            var total = tagsList.Count;

            tagsList = tagsList
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagingDto<ModelManager>(total, tagsList);
        }
        else
        {
            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<ModelManager>(total, data);
        }
    }

    /// <summary>
    /// 获取模型库元数据信息（包含tags、types、providers、icons等）
    /// </summary>
    /// <param name="context">HTTP上下文</param>
    /// <returns></returns>
    public static async Task<object> GetModelLibraryMetadataAsync(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();
        
        var models = await dbContext.ModelManagers
            .AsNoTracking()
            .Where(x => x.Enable) // 只获取启用的模型
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