using AIDotNet.Abstractions;
using AIDotNet.ErnieBot;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddErnieBot(this IServiceCollection services)
    {
        IApiChatCompletionService.ServiceNames.Add("百度ErnieBot", ErnieBotOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, ErnieBotService>(ErnieBotOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, ErnieBotTextEmbeddingGeneration>(ErnieBotOptions
            .ServiceName);
        return services;
    }
}