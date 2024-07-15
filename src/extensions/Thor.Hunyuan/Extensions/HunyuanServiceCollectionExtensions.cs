using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Hunyuan;
using Thor.Hunyuan.Chats;
using Thor.Hunyuan.Embeddings;

namespace Thor.Hunyuan.Extensions;

public static class HunyuanServiceCollectionExtensions
{
    /// <summary>
    /// 添加腾讯混元平台支持
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHunyuanService(this IServiceCollection services)
    {
        // 添加平台名和编码对应
        ThorGlobal.PlatformNames.Add(HunyuanPlatformOptions.PlatformName, HunyuanPlatformOptions.PlatformCode);

        // 添加平台支持模型列表
        ThorGlobal.ModelNames.Add(HunyuanPlatformOptions.PlatformCode, [
            "hunyuan-lite",
            "hunyuan-standard",
            "hunyuan-pro",
            "hunyuan-role",
            "hunyuan-code"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, HunyuanChatCompletionsService>(HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, HunyuanTextEmbeddingService>(HunyuanPlatformOptions
            .PlatformCode);
        return services;
    }
}