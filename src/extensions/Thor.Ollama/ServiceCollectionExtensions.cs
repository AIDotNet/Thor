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
        IApiChatCompletionService.ServiceNames.Add("Ollama", OllamaOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, OllamaChatService>(OllamaOptions.ServiceName);
        return services;
    }
}