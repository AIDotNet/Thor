using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.Moonshot.Chats;

namespace Thor.Moonshot.Extensions;

/// <summary>
/// 月之暗面服务扩展
/// </summary>
public static class MoonshotServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMoonshotService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(MoonshotPlatformOptions.PlatformName, MoonshotPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(MoonshotPlatformOptions.PlatformCode, [
            "moonshot-v1-8k",
            "moonshot-v1-32k",
            "moonshot-v1-128k",
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, MoonshotChatCompletionsService>(MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, MoonshotServiceTextEmbeddingGeneration>(
            MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, MoonshotServiceImageService>(MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorCompletionsService, MoonshotServiceCompletionService>(MoonshotPlatformOptions
            .PlatformCode);

        services.AddHttpClient(MoonshotPlatformOptions.PlatformCode,
                options => { options.Timeout = TimeSpan.FromMinutes(6); })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxConnectionsPerServer = 300,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
                PooledConnectionLifetime = TimeSpan.FromMinutes(30),
                EnableMultipleHttp2Connections = true,
            });

        return services;
    }
}