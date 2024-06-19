using Thor.Abstractions;
using Thor.MetaGLM;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMClientV4(this IServiceCollection services)
    {

        IApiChatCompletionService.ServiceNames.Add("智谱 AI", MetaGLMOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, MetaGLMService>(MetaGLMOptions.ServiceName);
        return services;
    }
}