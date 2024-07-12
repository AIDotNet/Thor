using Thor.Abstractions;
using Thor.Hunyuan;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHunyuan(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(HunyuanPlatformOptions.PlatformName, HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IChatCompletionsService, HunyuanService>(HunyuanPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, HunyuanTextEmbeddingGeneration>(HunyuanPlatformOptions
            .PlatformCode);
        return services;
    }
}