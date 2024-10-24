using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.OpenAI.Chats;
using Thor.OpenAI.Embeddings;

namespace Thor.OpenAI.Extensions;

public static class AIGiteeCollectionExtensions
{
    public static IServiceCollection AddAIGiteeService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(AIGiteePlatformOptions.PlatformName, AIGiteePlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(AIGiteePlatformOptions.PlatformCode, [
            "deepseek-coder-33B-instruct"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, AIGiteeChatCompletionsService>(AIGiteePlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, AIGiteeTextEmbeddingService>(
            AIGiteePlatformOptions.PlatformCode);
        
        // services.AddKeyedSingleton<IThorImageService, OpenAIImageService>(AIGiteePlatformOptions.PlatformCode);
        //
        services.AddKeyedSingleton<IThorCompletionsService, AIGiteeCompletionService>(AIGiteePlatformOptions
            .PlatformCode);

        services.AddHttpClient(AIGiteePlatformOptions.PlatformCode,
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