using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.OpenAI.Chats;
using Thor.OpenAI.Embeddings;

namespace Thor.OpenAI.Extensions;

public static class GiteeAICollectionExtensions
{
    public static IServiceCollection AddGiteeAIService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(GiteeAIPlatformOptions.PlatformName, GiteeAIPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(GiteeAIPlatformOptions.PlatformCode, [
            "deepseek-coder-33B-instruct"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, GiteeAIChatCompletionsService>(GiteeAIPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, GiteeAITextEmbeddingService>(
            GiteeAIPlatformOptions.PlatformCode);
        
        // services.AddKeyedSingleton<IThorImageService, OpenAIImageService>(AIGiteePlatformOptions.PlatformCode);
        //
        services.AddKeyedSingleton<IThorCompletionsService, GiteeAICompletionService>(GiteeAIPlatformOptions
            .PlatformCode);

        services.AddHttpClient(GiteeAIPlatformOptions.PlatformCode,
                options =>
                {
                    options.Timeout = TimeSpan.FromMinutes(6);
                    
                    options.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(6),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(6),
            });

        return services;
    }
}