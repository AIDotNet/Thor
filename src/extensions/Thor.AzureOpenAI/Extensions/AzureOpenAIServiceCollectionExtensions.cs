using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.Abstractions.Realtime;
using Thor.AzureOpenAI.Chats;
using Thor.AzureOpenAI.Embeddings;
using Thor.AzureOpenAI.Realtime;

namespace Thor.AzureOpenAI.Extensions;

public static class AzureOpenAIServiceCollectionExtensions
{
    public static IServiceCollection AddAzureOpenAIService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(AzureOpenAIPlatformOptions.PlatformName, AzureOpenAIPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(AzureOpenAIPlatformOptions.PlatformCode, [
            "gpt-3.5-turbo",
            "gpt-3.5-turbo-0125",
            "gpt-3.5-turbo-0301",
            "gpt-3.5-turbo-0613",
            "gpt-3.5-turbo-1106",
            "gpt-3.5-turbo-16k",
            "gpt-3.5-turbo-16k-0613",
            "gpt-3.5-turbo-instruct",
            "gpt-4",
            "gpt-4-0125-preview",
            "gpt-4-0314",
            "gpt-4-0613",
            "gpt-4-1106-preview",
            "gpt-4-32k",
            "gpt-4-32k-0314",
            "gpt-4-32k-0613",
            "gpt-4-turbo-preview",
            "gpt-4-vision-preview",
            "text-embedding-3-large",
            "text-embedding-3-small",
            "text-embedding-ada-002",
            "text-embedding-v1",
            "text-moderation-latest",
            "text-moderation-stable",
            "text-search-ada-doc-001"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, AzureOpenAIChatCompletionsService>(
            AzureOpenAIPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IThorTextEmbeddingService, AzureOpenAITextEmbeddingGenerationService>(
            AzureOpenAIPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IThorImageService, AzureOpenAIServiceImageService>(AzureOpenAIPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorRealtimeService, AzureOpenAIRealtimeService>(AzureOpenAIPlatformOptions
            .PlatformCode);

        services.AddHttpClient(AzureOpenAIPlatformOptions.PlatformCode,
                options =>
                {
                    options.Timeout = TimeSpan.FromMinutes(6);

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