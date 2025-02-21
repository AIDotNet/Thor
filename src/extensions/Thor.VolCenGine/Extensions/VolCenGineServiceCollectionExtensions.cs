using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.SiliconFlow;
using Thor.SiliconFlow.Embeddings;
using Thor.VolCenGine.Chats;

namespace Thor.VolCenGine.Extensions;

public static class VolCenGineServiceCollectionExtensions
{
    public static IServiceCollection AddVolCenGineService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(VolCenGinePlatformOptions.PlatformName, VolCenGinePlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(VolCenGinePlatformOptions.PlatformCode, [
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, VolCenGineChatCompletionsService>(VolCenGinePlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, VolCenGineTextEmbeddingService>(
            VolCenGinePlatformOptions.PlatformCode);
        
        return services;
    }
}