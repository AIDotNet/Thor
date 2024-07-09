using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Thor.Abstractions;

namespace Thor.Moonshot;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMoonshotService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(MoonshotPlatformOptions.PlatformName, MoonshotPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(MoonshotPlatformOptions.PlatformCode, [
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0125",
            "gpt-3.5-turbo-0301",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-1106",
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-16k-0613",
            "gpt-3.5-turbo-instruct",
        ]);

        services.AddKeyedSingleton<IApiChatCompletionService, MoonshotService>(MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, MoonshotServiceTextEmbeddingGeneration>(
            MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiImageService, MoonshotServiceImageService>(MoonshotPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiCompletionService, MoonshotServiceCompletionService>(MoonshotPlatformOptions
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