using AIDotNet.Abstractions;
using AIDotNet.OpenAI;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        IApiChatCompletionService.ServiceNames.Add("OpenAI", OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, OpenAiService>(OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, OpenAIServiceTextEmbeddingGeneration>(
            OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiImageService, OpenAIServiceImageService>(OpenAIServiceOptions.ServiceName);
        return services;
    }
}