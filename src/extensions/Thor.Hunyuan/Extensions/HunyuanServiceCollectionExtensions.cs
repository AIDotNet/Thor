using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Hunyuan;
using Thor.Hunyuan.Chats;
using Thor.Hunyuan.Embeddings;

namespace Thor.Hunyuan.Extensions;

public static class HunyuanServiceCollectionExtensions
{
    public static IServiceCollection AddHunyuan(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(HunyuanPlatformOptions.PlatformName, HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, HunyuanChatCompletionsService>(HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, HunyuanTextEmbeddingService>(HunyuanPlatformOptions
            .PlatformCode);
        return services;
    }
}