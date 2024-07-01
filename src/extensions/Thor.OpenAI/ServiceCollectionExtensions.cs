using System.Net;
using Thor.Abstractions;
using Thor.OpenAI;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        IApiChatCompletionService.ServiceNames.Add("OpenAI", OpenAIServiceOptions.ServiceName);
        IApiChatCompletionService.ServiceNames.Add("Kimi（月之暗面）", "Kimi" + OpenAIServiceOptions.ServiceName);
        IApiChatCompletionService.ModelNames.Add("OpenAI", [
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

        services.AddKeyedSingleton<IApiChatCompletionService, OpenAiService>("Kimi" + OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, OpenAIServiceTextEmbeddingGeneration>(
            "Kimi" + OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiImageService, OpenAIServiceImageService>("Kimi" +
            OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiCompletionService, OpenAIServiceCompletionService>("Kimi" + OpenAIServiceOptions
            .ServiceName);

        services.AddKeyedSingleton<IApiChatCompletionService, OpenAiService>(OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, OpenAIServiceTextEmbeddingGeneration>(
            OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiImageService, OpenAIServiceImageService>(OpenAIServiceOptions.ServiceName);
        services.AddKeyedSingleton<IApiCompletionService, OpenAIServiceCompletionService>(OpenAIServiceOptions
            .ServiceName);

        services.AddHttpClient(OpenAIServiceOptions.ServiceName,
                options => { options.Timeout = TimeSpan.FromMinutes(6); })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                EnableMultipleHttp2Connections = true,
            });

        return services;
    }
}