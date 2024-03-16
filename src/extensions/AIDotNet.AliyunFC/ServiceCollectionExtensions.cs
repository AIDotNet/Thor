using AIDotNet.Abstractions;
using AIDotNet.AliyunFC;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAliyunFCService(this IServiceCollection services,
        Action<AliyunFCOptions>? configureOptions = null,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddKeyedSingleton<IADNChatCompletionService>(AliyunFCOptions.ServiceName,
                    (provider, name) =>
                    {
                        var aliyunFcOptions = new AliyunFCOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(aliyunFcOptions);
                        aliyunFcOptions.HttpClient ??= new HttpClient();
                        return new AliyunFCService(aliyunFcOptions);
                    });
                break;
            case ServiceLifetime.Scoped:
                services.AddKeyedScoped<IADNChatCompletionService>(AliyunFCOptions.ServiceName,
                    (provider, name) =>
                    {
                        var aliyunFcOptions = new AliyunFCOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(aliyunFcOptions);
                        aliyunFcOptions.HttpClient ??= new HttpClient();
                        return new AliyunFCService(aliyunFcOptions);
                    });
                break;
            case ServiceLifetime.Transient:
                services.AddKeyedTransient<IADNChatCompletionService>(AliyunFCOptions.ServiceName,
                    (provider, name) =>
                    {
                        var aliyunFcOptions = new AliyunFCOptions
                        {
                            ServiceProvider = provider
                        };
                        configureOptions?.Invoke(aliyunFcOptions);
                        aliyunFcOptions.HttpClient ??= new HttpClient();
                        return new AliyunFCService(aliyunFcOptions);
                    });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
        }

        return services;
    }
}