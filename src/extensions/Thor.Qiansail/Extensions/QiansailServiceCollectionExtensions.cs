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
        /// <summary>
        /// 添加通义千问平台支持
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQiansailService(this IServiceCollection services)
        {
            // 添加平台名和编码对应
            ThorGlobal.PlatformNames.Add(QiansailPlatformOptions.PlatformName, QiansailPlatformOptions.PlatformCode);

            // 添加平台支持模型列表
            ThorGlobal.ModelNames.Add(QiansailPlatformOptions.PlatformCode, [
                "qwen-plus",
                "qwen-max",
                "qwen-turbo",
            ]);

            services.AddKeyedSingleton<IThorChatCompletionsService, QiansailChatCompletionsService>(QiansailPlatformOptions.PlatformCode);
            services.AddKeyedSingleton<IThorTextEmbeddingService, QiansailTextEmbeddingService>(QiansailPlatformOptions
                .PlatformCode);
            return services;
        }
    }
}