using Thor.Abstractions;
using Thor.SparkDesk;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(SparkDeskPlatformOptions.PlatformName, SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IChatCompletionsService, SparkDeskChatCompletionsService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiImageService, SparkDeskImageService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, SparkDeskTextEmbeddingGeneration>(SparkDeskPlatformOptions.PlatformCode);

        return services;
    }
}