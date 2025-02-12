using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.DeepSeek.Chats;

namespace Thor.DeepSeek.Extensions;

public static class DeepSeekServiceCollectionExtensions
{
    public static IServiceCollection AddDeepSeekService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(DeepSeekPlatformOptions.PlatformName, DeepSeekPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(DeepSeekPlatformOptions.PlatformCode, [
            "deepseek-chat",
            "deepseek-reasoner"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, DeepSeekChatCompletionsService>(DeepSeekPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorCompletionsService, DeepSeekCompletionService>(DeepSeekPlatformOptions
            .PlatformCode);

        services.AddHttpClient(DeepSeekPlatformOptions.PlatformCode,
                options =>
                {
                    options.Timeout = TimeSpan.FromMinutes(10);

                    options.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(6),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(6),
                EnableMultipleHttp2Connections = true,
                ConnectTimeout = TimeSpan.FromMinutes(6)
            });

        return services;
    }
}