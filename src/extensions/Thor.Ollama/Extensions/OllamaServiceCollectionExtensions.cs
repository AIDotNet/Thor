using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Ollama;
using Thor.Ollama.Chats;

namespace Thor.Ollama.Extensions;

public static class OllamaServiceCollectionExtensions
{
    /// <summary>
    /// 添加 Ollama 平台支持
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOllamaService(this IServiceCollection services)
    {
        // 添加平台名和编码对应
        ThorGlobal.PlatformNames.Add(OllamaPlatformOptions.PlatformName, OllamaPlatformOptions.PlatformCode);

        // 添加平台支持模型列表
        ThorGlobal.ModelNames.Add(OllamaPlatformOptions.PlatformCode, [
            "llama2:latest",
            "llama3:latest",
            "qwen:4b",
            "qwen2.5",
            "qwen2.5:32b"
        ]);

        // 基于平台码注册服务
        services.AddKeyedSingleton<IThorChatCompletionsService, OllamaChatCompletionsService>(OllamaPlatformOptions.PlatformCode);

        return services;
    }
}