using Thor.Abstractions;
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

        services.AddKeyedSingleton<IChatCompletionsService, OllamaChatService>(OllamaPlatformOptions.PlatformCode);
        return services;
    }
}