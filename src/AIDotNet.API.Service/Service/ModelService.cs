using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain.Core;
using AIDotNet.API.Service.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AIDotNet.API.Service.Service;

public static class ModelService
{
    /// <summary>
    /// 获取模型
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> GetTypes()
        => IApiChatCompletionService.ServiceNames;

    public static string[] GetModels()
    {
        var result = SettingService.PromptRate.Select(x => x.Key).ToArray();

        return result;
    }

    public static async ValueTask<List<UseModelDto>> GetUseModels(HttpContext context)
    {
        var cache = context.RequestServices.GetRequiredService<IMemoryCache>();

        if (cache.TryGetValue("UseModels", out List<UseModelDto> model))
        {
            return model;
        }

        var dbContext = context.RequestServices.GetRequiredService<AIDotNetDbContext>();
        var loggerDbContext = context.RequestServices.GetRequiredService<LoggerDbContext>();

        // 获取模型
        var channels = await dbContext.Channels.ToListAsync();

        var value = channels.SelectMany(x => x.Models).Distinct()
            .Select(x => new UseModelDto()
            {
                Model = x
            }).ToList();

        var now = DateTime.Now.AddDays(7);

        foreach (var item in value)
        {
            var count = await loggerDbContext.Loggers
                .Where(x => x.CreatedAt > now && x.ModelName == item.Model && x.Type == ChatLoggerType.Consume)
                .CountAsync();

            item.Count = count;
        }

        value = value.OrderByDescending(x => x.Count).ToList();

        // 修改前三模型
        for (var i = 0; i < value.Count; i++)
        {
            if (i == 3)
            {
                break;
            }

            if (value[i].Count > 500)
            {
                value[i].Hot = true;
            }
        }

        cache.Set("UseModels", value, TimeSpan.FromHours(5));

        return value;
    }
}