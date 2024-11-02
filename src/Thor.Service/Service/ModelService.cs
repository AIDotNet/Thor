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
        var result = ModelManagerService.PromptRate.Select(x => x.Key).ToArray();

        return result;
    }

    public static async ValueTask<List<UseModelDto>> GetUseModels(HttpContext context)
    {
        var cache = context.RequestServices.GetRequiredService<IServiceCache>();

        var model = await cache.GetAsync<List<UseModelDto>>("UseModels").ConfigureAwait(false);
        if (model != null) return model;

        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();
        var loggerDbContext = context.RequestServices.GetRequiredService<ILoggerDbContext>();

        // 获取模型
        var channels = await dbContext.Channels.ToListAsync();

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

        await cache.CreateAsync("UseModels", value, TimeSpan.FromHours(5));

        return value;
    }

    public static async Task<ModelsListDto> GetAsync(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<IThorContext>();

        var models = await dbContext.ModelManagers
            .OrderBy(x => x.CreatedAt)
            .Where(x => x.Enable)
            .ToListAsync();

        var modelsListDto = new ModelsListDto();

        foreach (var model in models)
        {
            modelsListDto.Data.Add(new ModelsDataDto()
            {
                Created = model.CreatedAt.ToUnixTimeSeconds(),
                Id = model.Model,
                @object = "model",
                OwnedBy = "openai",
            });
        }

        return modelsListDto;
    }
}