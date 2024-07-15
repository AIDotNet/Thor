using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.MetaGLM;
using Thor.MetaGLM.Chats;

namespace Thor.MetaGLM.Extensions;

public static class MetaGLMServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMService(this IServiceCollection services)
    {

        ThorGlobal.PlatformNames.Add(MetaGLMPlatformOptions.PlatformName, MetaGLMPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, MetaGLMChatCompletionsService>(MetaGLMPlatformOptions.PlatformCode);
        return services;
    }
}