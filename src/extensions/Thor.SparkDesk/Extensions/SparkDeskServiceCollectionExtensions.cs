using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.SparkDesk.Chats;
using Thor.SparkDesk.Embeddings;
using Thor.SparkDesk.Images;

namespace Thor.SparkDesk.Extensions;

public static class SparkDeskServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(SparkDeskPlatformOptions.PlatformName, SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, SparkDeskChatCompletionsService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, SparkDeskImageService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, SparkDeskTextEmbeddingService>(SparkDeskPlatformOptions.PlatformCode);

        return services;
    }
}