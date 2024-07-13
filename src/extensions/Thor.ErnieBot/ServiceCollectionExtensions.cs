using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.ErnieBot;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddErnieBot(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(ErnieBotPlatformOptions.PlatformName, ErnieBotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, ErnieBotChatCompletionsService>(ErnieBotPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, ErnieBotTextEmbeddingGeneration>(ErnieBotPlatformOptions
            .PlatformCode);
        return services;
    }
}