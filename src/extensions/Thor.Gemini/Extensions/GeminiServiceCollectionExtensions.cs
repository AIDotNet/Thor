using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.Gemini.Chats;
using Thor.Gemini.Embeddings;
using Thor.OpenAI.Images;

namespace Thor.Gemini.Extensions;

public static class GeminiServiceCollectionExtensions
{
    public static IServiceCollection AddGeminiService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(GeminiPlatformOptions.PlatformName, GeminiPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(GeminiPlatformOptions.PlatformCode, [
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, GeminiChatCompletionsService>(GeminiPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, GeminiTextEmbeddingService>(
            GeminiPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, GeminiImageService>(GeminiPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorCompletionsService, GeminiCompletionService>(GeminiPlatformOptions
            .PlatformCode);

        services.AddHttpClient(GeminiPlatformOptions.PlatformCode,
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