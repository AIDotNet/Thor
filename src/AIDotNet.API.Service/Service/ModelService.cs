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
        var result = SettingService.PromptRate.Select(x => x.Key).ToArray();

        return result;
    }
}