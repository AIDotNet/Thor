using Thor.Abstractions;
using Thor.MetaGLM;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMClientV4(this IServiceCollection services)
    {

        ThorGlobal.PlatformNames.Add(MetaGLMPlatformOptions.PlatformName, MetaGLMPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IChatCompletionsService, MetaGLMService>(MetaGLMPlatformOptions.PlatformCode);
        return services;
    }
}