using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.MiniMax.Chats;

namespace Thor.MiniMax.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// 添加月之暗面平台支持
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMiniMaxService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(MiniMaxPlatformOptions.PlatformName, MiniMaxPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(MiniMaxPlatformOptions.PlatformCode, [
            "abab6.5s-chat",
            "MiniMax-Text-01"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, MiniMaxChatCompletionsService>(MiniMaxPlatformOptions
            .PlatformCode);

        return services;
    }
}