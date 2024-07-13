using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Ollama;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOllamaService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(OllamaPlatformOptions.PlatformName, OllamaPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, OllamaChatCompletionsService>(OllamaPlatformOptions.PlatformCode);
        return services;
    }
}