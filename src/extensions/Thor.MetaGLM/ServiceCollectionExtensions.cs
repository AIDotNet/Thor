using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.MetaGLM;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMClientV4(this IServiceCollection services)
    {

        ThorGlobal.PlatformNames.Add(MetaGLMPlatformOptions.PlatformName, MetaGLMPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorChatCompletionsService, MetaGLMChatCompletionsService>(MetaGLMPlatformOptions.PlatformCode);
        return services;
    }
}