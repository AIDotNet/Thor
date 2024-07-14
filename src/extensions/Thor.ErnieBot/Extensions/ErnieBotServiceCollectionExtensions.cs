using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.ErnieBot.Chats;
using Thor.ErnieBot.Embeddings;

namespace Thor.ErnieBot.Extensions;

public static class ErnieBotServiceCollectionExtensions
{
    public static IServiceCollection AddErnieBot(this IServiceCollection services)
    {
        //添加平台名和编码对应
        ThorGlobal.PlatformNames.Add(ErnieBotPlatformOptions.PlatformName, ErnieBotPlatformOptions.PlatformCode);

        //添加平台支持模型列表
        ThorGlobal.ModelNames.Add(ErnieBotPlatformOptions.PlatformCode, [
            "ERNIE-Speed-8K",
            "ERNIE-Lite-8K",
            "ERNIE-Speed-128K",
        ]);

        //基于平台码注册服务
        services.AddKeyedSingleton<IThorChatCompletionsService, ErnieBotChatCompletionsService>(ErnieBotPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IThorTextEmbeddingService, ErnieBotTextEmbeddingService>(ErnieBotPlatformOptions.PlatformCode);

        return services;
    }
}