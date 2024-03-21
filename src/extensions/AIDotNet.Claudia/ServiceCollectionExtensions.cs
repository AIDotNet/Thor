using AIDotNet.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Claudia
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClaudia(this IServiceCollection services,
            Action<ClaudiaOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {

            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddKeyedSingleton<IADNChatCompletionService>(ClaudiaOptions.ServiceName,
                        (provider, name) =>
                        {
                            var openAIOptions = new ClaudiaOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(openAIOptions);
                            return new ClaudiaService(openAIOptions);
                        });
                    break;
                case ServiceLifetime.Scoped:
                    services.AddKeyedScoped<IADNChatCompletionService>(ClaudiaOptions.ServiceName,
                        (provider, name) =>
                        {
                            var openAIOptions = new ClaudiaOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(openAIOptions);
                            return new ClaudiaService(openAIOptions);
                        });
                    break;
                case ServiceLifetime.Transient:
                    services.AddKeyedTransient<IADNChatCompletionService>(ClaudiaOptions.ServiceName,
                        (provider, name) =>
                        {
                            var openAIOptions = new ClaudiaOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(openAIOptions);
                            return new ClaudiaService(openAIOptions);
                        });
                    break;

            }

            return services;
        }
    }
}
