using AIDotNet.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AIDotNet.Qiansail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQiansail(this IServiceCollection services, Action<QiansailOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddKeyedSingleton<IADNChatCompletionService>(QiansailOptions.ServiceName,
                        (provider, name) =>
                        {
                            var qiansailOptions = new QiansailOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(qiansailOptions);

                            return new QiansailService(qiansailOptions);
                        });
                    break;
                case ServiceLifetime.Scoped:
                    services.AddKeyedScoped<IADNChatCompletionService>(QiansailOptions.ServiceName,
                        (provider, name) =>
                        {
                            var qiansailOptions = new QiansailOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(qiansailOptions);

                            return new QiansailService(qiansailOptions);
                        });
                    break;
                case ServiceLifetime.Transient:
                    services.AddKeyedTransient<IADNChatCompletionService>(QiansailOptions.ServiceName,
                        (provider, name) =>
                        {
                            var qiansailOptions = new QiansailOptions
                            {
                                ServiceProvider = provider
                            };
                            configureOptions?.Invoke(qiansailOptions);

                            return new QiansailService(qiansailOptions);
                        });
                    break;

            }

            return services;
        }
    }
}
