using AIDotNet.Abstractions;
using AIDotNet.SparkDesk;
using Sdcb.SparkDesk;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkDeskService(this IServiceCollection services,
        Action<SparkDeskOptions>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        IADNChatCompletionService.ServiceNames.Add("星火大模型", SparkDeskOptions.ServiceName);

        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton<IADNChatCompletionService>(SparkDeskOptions.ServiceName,
                    (provider, name) =>
                    {
                        var deskOptions = new SparkDeskOptions
                        {
                            ServiceProvider = provider
                        };

                        configureOptions?.Invoke(deskOptions);

                        return new SparkDeskService(deskOptions);
                    });
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped<IADNChatCompletionService>(SparkDeskOptions.ServiceName,
                    (provider, name) =>
                    {
                        var deskOptions = new SparkDeskOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(deskOptions);

                        return new SparkDeskService(deskOptions);
                    });
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient<IADNChatCompletionService>(SparkDeskOptions.ServiceName,
                    (provider, name) =>
                    {
                        var deskOptions = new SparkDeskOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(deskOptions);

                        return new SparkDeskService(deskOptions);
                    });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }
}