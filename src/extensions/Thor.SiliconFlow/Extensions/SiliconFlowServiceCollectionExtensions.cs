using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Audios;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Images;
using Thor.SiliconFlow.Audios;
using Thor.SiliconFlow.Chats;
using Thor.SiliconFlow.Embeddings;
using Thor.SiliconFlow.Images;

namespace Thor.SiliconFlow.Extensions;

public static class SiliconFlowServiceCollectionExtensions
{
    public static IServiceCollection AddSiliconFlowService(this IServiceCollection services)
    {
        ThorGlobal.PlatformNames.Add(SiliconFlowPlatformOptions.PlatformName, SiliconFlowPlatformOptions.PlatformCode);

        ThorGlobal.ModelNames.Add(SiliconFlowPlatformOptions.PlatformCode, [
            "deepseek-ai/DeepSeek-R1",
            "deepseek-ai/DeepSeek-V3",
            "Pro/deepseek-ai/DeepSeek-R1",
            "Pro/deepseek-ai/DeepSeek-V3",
            "deepseek-ai/DeepSeek-R1-Distill-Llama-70B",
            "deepseek-ai/DeepSeek-R1-Distill-Qwen-32B",
            "deepseek-ai/DeepSeek-R1-Distill-Qwen-14B",
            "deepseek-ai/DeepSeek-R1-Distill-Llama-8B",
            "deepseek-ai/DeepSeek-R1-Distill-Qwen-7B",
            "deepseek-ai/DeepSeek-R1-Distill-Qwen-1.5B",
            "deepseek-ai/Janus-Pro-7B",
            "Qwen/QVQ-72B-Preview"
        ]);

        services.AddKeyedSingleton<IThorChatCompletionsService, SiliconFlowChatCompletionsService>(SiliconFlowPlatformOptions
            .PlatformCode);

        services.AddKeyedSingleton<IThorTextEmbeddingService, SiliconFlowTextEmbeddingService>(
            SiliconFlowPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorImageService, SiliconFlowImageService>(SiliconFlowPlatformOptions.PlatformCode);

        services.AddKeyedSingleton<IThorCompletionsService, SiliconFlowCompletionService>(SiliconFlowPlatformOptions
            .PlatformCode);
        
        services.AddKeyedSingleton<IThorAudioService, SiliconFlowAudioService>(SiliconFlowPlatformOptions
            .PlatformCode);

        services.AddHttpClient(SiliconFlowPlatformOptions.PlatformCode,
                options =>
                {
                    options.Timeout = TimeSpan.FromMinutes(6);

                    options.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(6),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(6),
                EnableMultipleHttp2Connections = true,
                ConnectTimeout = TimeSpan.FromMinutes(6)
            });

        return services;
    }
}