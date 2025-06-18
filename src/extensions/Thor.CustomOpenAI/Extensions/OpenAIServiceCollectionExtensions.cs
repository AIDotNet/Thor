using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Audios;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.Abstractions.Responses;
using Thor.CustomOpenAI.Audios;
using Thor.CustomOpenAI.Chats;
using Thor.CustomOpenAI.Images;
using Thor.CustomOpenAI.Responses;
using Thor.OpenAI.Embeddings;

namespace Thor.CustomOpenAI.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IServiceCollection AddCustomeOpenAIService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(CustomeOpenAIPlatformOptions.PlatformName, CustomeOpenAIPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(CustomeOpenAIPlatformOptions.PlatformCode, [
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
            "gpt-4-all",
            "gpt-4-gizmo-*",
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

        services.AddKeyedSingleton<IThorChatCompletionsService, OpenAIChatCompletionsService>(CustomeOpenAIPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, OpenAITextEmbeddingService>(
            CustomeOpenAIPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, OpenAIImageService>(CustomeOpenAIPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorCompletionsService, OpenAICompletionService>(CustomeOpenAIPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorAudioService, OpenAIAudioService>(CustomeOpenAIPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorResponsesService, OpenAIResponsesService>(CustomeOpenAIPlatformOptions.PlatformCode);

        services.AddHttpClient(CustomeOpenAIPlatformOptions.PlatformCode,
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