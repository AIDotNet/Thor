using AIDotNet.Abstractions;
using AIDotNet.OpenAI;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services,
        Action<OpenAIOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        IADNChatCompletionService.ServiceNames.Add("OpenAI", OpenAIOptions.ServiceName);
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
                        openAIOptions.Client ??= new HttpClient();

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
                        openAIOptions.Client ??= new HttpClient();

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
                        openAIOptions.Client ??= new HttpClient();
                        return new OpenAiService(openAIOptions);
                    });
                break;

        }

        return services;
    }
}