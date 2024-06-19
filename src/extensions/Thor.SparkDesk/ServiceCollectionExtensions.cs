using Thor.Abstractions;
using Thor.SparkDesk;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services)
    {
        IApiChatCompletionService.ServiceNames.Add("星火大模型", SparkDeskOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, SparkDeskService>(SparkDeskOptions.ServiceName);
        services.AddKeyedSingleton<IApiImageService, SparkDeskImageService>(SparkDeskOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, SparkDeskTextEmbeddingGeneration>(SparkDeskOptions.ServiceName);
        return services;
    }
}