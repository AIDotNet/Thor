using System.Text.Json;
using AIDotNet.Abstractions;

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
        if (!File.Exists("model.json"))
        {
            return [];
        }

        var result = JsonSerializer.Deserialize<string[]>(File.ReadAllText("model.json"));

        return result;
    }
}