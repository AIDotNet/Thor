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
        //添加 Ollama 平台支持
        ThorGlobal.PlatformNames.Add(SparkDeskPlatformOptions.PlatformName, SparkDeskPlatformOptions.PlatformCode);

        // 添加平台支持模型列表
        ThorGlobal.ModelNames.Add(SparkDeskPlatformOptions.PlatformCode, [
            "SparkDesk-V1.1(Lite)",
            "SparkDesk-V2.1(V2.0)",
            "SparkDesk-V3.1(Pro)",
            "SparkDesk-V3.5(Max)",
            "SparkDesk-V4.0(Ultra)",
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, SparkDeskChatCompletionsService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, SparkDeskImageService>(SparkDeskPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, SparkDeskTextEmbeddingService>(SparkDeskPlatformOptions.PlatformCode);

        return services;
    }
}