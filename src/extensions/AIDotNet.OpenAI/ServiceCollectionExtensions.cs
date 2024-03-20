using AIDotNet.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.OpenAI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services,
        Action<OpenAIOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton<IADNChatCompletionService>(OpenAIOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new OpenAIOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);

                        return new OpenAiService(openAIOptions);
                    });
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped<IADNChatCompletionService>(OpenAIOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new OpenAIOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);

                        return new OpenAiService(openAIOptions);
                    });
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient<IADNChatCompletionService>(OpenAIOptions.ServiceName,
                    (provider, name) =>
                    {
                        var openAIOptions = new OpenAIOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(openAIOptions);

                        return new OpenAiService(openAIOptions);
                    });
                break;

        }

        return services;
    }
}