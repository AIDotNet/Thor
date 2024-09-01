using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.MetaGLM;
using Thor.MetaGLM.Chats;
using Thor.MetaGLM.Embeddings;

namespace Thor.MetaGLM.Extensions;

public static class MetaGLMServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMService(this IServiceCollection services)
    {

        ThorGlobal.PlatformNames.Add(MetaGLMPlatformOptions.PlatformName, MetaGLMPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, MetaGLMChatCompletionsService>(MetaGLMPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IThorTextEmbeddingService, MetaGLMTextEmbeddingService>(MetaGLMPlatformOptions.PlatformCode);
        
        return services;
    }
}