using AIDotNet.Abstractions;
using AIDotNet.MetaGLM;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaGLMClientV4(this IServiceCollection services,
        Action<MetaGLMOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {

        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton<IADNChatCompletionService>(MetaGLMOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new MetaGLMOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);
                        openAIOptions.Client ??= new ();

                        return new MetaGLMService(openAIOptions);
                    });
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped<IADNChatCompletionService>(MetaGLMOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new MetaGLMOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);
                        openAIOptions.Client ??= new ();

                        return new MetaGLMService(openAIOptions);
                    });
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient<IADNChatCompletionService>(MetaGLMOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new MetaGLMOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);
                        openAIOptions.Client ??= new ();

                        return new MetaGLMService(openAIOptions);
                    });
                break;

        }

        return services;
    }
}