using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Hunyuan;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHunyuan(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(HunyuanPlatformOptions.PlatformName, HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, HunyuanChatCompletionsService>(HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, HunyuanTextEmbeddingGeneration>(HunyuanPlatformOptions
            .PlatformCode);
        return services;
    }
}