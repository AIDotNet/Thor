using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;

namespace Thor.Qiansail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQiansail(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(QiansailPlatformOptions.PlatformName, QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IThorChatCompletionsService, QiansailChatCompletionsService>(QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IThorTextEmbeddingService, QiansailTextEmbeddingGeneration>(QiansailPlatformOptions
                .PlatformCode);
            return services;
        }
    }
}