using Thor.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Qiansail.Chats;
using Thor.Qiansail.Embeddings;

namespace Thor.Qiansail.Extensions
{
    public static class QiansailServiceCollectionExtensions
    {
        public static IServiceCollection AddQiansail(this IServiceCollection services)
        {
            ThorGlobal.PlatformNames.Add(QiansailPlatformOptions.PlatformName, QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IThorChatCompletionsService, QiansailChatCompletionsService>(QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IThorTextEmbeddingService, QiansailTextEmbeddingService>(QiansailPlatformOptions
                .PlatformCode);
            return services;
        }
    }
}