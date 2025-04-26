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
}