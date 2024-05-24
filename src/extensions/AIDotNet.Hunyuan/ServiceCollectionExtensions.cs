using AIDotNet.Abstractions;
using AIDotNet.Hunyuan;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHunyuan(this IServiceCollection services)
    {
        IApiChatCompletionService.ServiceNames.Add("腾讯混元大模型", HunyuanOptions.ServiceName);
        services.AddKeyedSingleton<IApiChatCompletionService, HunyuanService>(HunyuanOptions.ServiceName);
        services.AddKeyedSingleton<IApiTextEmbeddingGeneration, HunyuanTextEmbeddingGeneration>(HunyuanOptions
            .ServiceName);
        return services;
    }
}