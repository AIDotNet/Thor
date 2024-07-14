using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.SparkDesk;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(SparkDeskPlatformOptions.PlatformName, SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, SparkDeskChatCompletionsService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, SparkDeskImageService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, SparkDeskTextEmbeddingGeneration>(SparkDeskPlatformOptions.PlatformCode);

        return services;
    }
}