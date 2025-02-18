using Microsoft.Extensions.DependencyInjection;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Embeddings;
using Thor.ErnieBot.Chats;
using Thor.ErnieBot.Embeddings;

namespace Thor.ErnieBot.Extensions;

public static class ErnieBotServiceCollectionExtensions
{
    public static IServiceCollection AddErnieBotService(this IServiceCollection services)
    {
        //添加平台名和编码对应
        ThorGlobal.PlatformNames.Add(ErnieBotPlatformOptions.PlatformName, ErnieBotPlatformOptions.PlatformCode);

        //添加平台支持模型列表
        ThorGlobal.ModelNames.Add(ErnieBotPlatformOptions.PlatformCode, [
            "ERNIE-4.0-8K",
            "ERNIE-4.0-8K-Latest",
            "ERNIE-4.0-8K-Preview",
            "ERNIE-4.0-8K-Preview-0518",
            "ERNIE-4.0-8K-0613",
            "ERNIE-4.0-8K-0329",
            "ERNIE-4.0-Turbo-8K",
            "ERNIE-3.5-128K",
            "ERNIE-3.5-8K",
            "ERNIE-3.5-8K-Preview",
            "ERNIE-3.5-8K-0613",
            "ERNIE-3.5-8K-0329",
            "ERNIE-Speed-128K",
            "ERNIE-Speed-8K",
            "ERNIE Speed-AppBuilder",
            "ERNIE-Character-8K",
            "ERNIE-Functions-8K",
            "ERNIE-Lite-8K",
            "ERNIE-Lite-8K-0922",
            "ERNIE-Lite-AppBuilder-8K",
            "ERNIE-Tiny-8K",
            "Qianfan-Chinese-Llama-2-7B",
            "Qianfan-Chinese-Llama-2-13B",
            "Qianfan-Chinese-Llama-2-70B",
            "Meta-Llama-3-8B",
            "Meta-Llama-3-70B",
            "Llama-2-7B-Chat",
            "Llama-2-13B-Chat",
            "Llama-2-70B-Chat",
            "ChatGLM2-6B-32K",
            "XuanYuan-70B-Chat-4bit",
            "Gemma-7B-It",
            "Yi-34B-Chat",
            "Mixtral-8x7B-Instruct",
            "ChatLaw",
            "Qianfan-BLOOMZ-7B-compressed",
            "BLOOMZ-7B",
            "AquilaChat-7B",
            "bge-large-en",
            "bge-large-zh",
            "tao-8k",
            "Embedding-V1",
            "ERNIE-Character-Fiction-8K",
            "ERNIE-4.0-Turbo-8K-Preview"
        ]);

        //基于平台码注册服务
        services.AddKeyedSingleton<IThorChatCompletionsService, ErnieChatV2CompletionsService>(ErnieBotPlatformOptions.PlatformCode);
        services.AddKeyedSingleton<IThorTextEmbeddingService, ErnieBotTextEmbeddingService>(ErnieBotPlatformOptions.PlatformCode);

        return services;
    }
}