using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Thor.Qiansail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQiansail(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(QiansailPlatformOptions.PlatformName, QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IApiChatCompletionService, QiansailService>(QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IApiTextEmbeddingGeneration, QiansailTextEmbeddingGeneration>(QiansailPlatformOptions
                .PlatformCode);
            return services;
        }
    }
}