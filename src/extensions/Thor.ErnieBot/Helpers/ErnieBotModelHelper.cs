using ERNIE_Bot.SDK;
using Thor.ErnieBot.Helpers.Dtos;

namespace Thor.ErnieBot.Helpers;

public class ErnieBotModelHelper
{
    /// <summary>
    /// 模型信息字典,key：模型编码，value：模型信息
    /// </summary>
    public static Dictionary<string, ErnieBotModelInfo> ModeInfoDict = new()
    {
        ["ERNIE-4.0-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K", Code = "completions_pro", Type = "chat" },
        ["ERNIE-4.0-8K-Latest"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K-Latest", Code = "ernie-4.0-8k-latest", Type = "chat" },
        ["ERNIE-4.0-8K-Preview"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K-Preview", Code = "ernie-4.0-8k-preview", Type = "chat" },
        ["ERNIE-4.0-8K-Preview-0518"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K-Preview-0518", Code = "completions_adv_pro", Type = "chat" },
        ["ERNIE-4.0-8K-0613"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K-0613", Code = "ernie-4.0-8k-0613", Type = "chat" },
        ["ERNIE-4.0-8K-0329"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-8K-0329", Code = "ernie-4.0-8k-0329", Type = "chat" },
        ["ERNIE-4.0-Turbo-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-Turbo-8K", Code = "ernie-4.0-turbo-8k", Type = "chat" },
        ["ERNIE-3.5-128K"] = new ErnieBotModelInfo() { Name = "ERNIE-3.5-128K", Code = "ernie-3.5-128k", Type = "chat" },
        ["ERNIE-3.5-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-3.5-8K", Code = "completions", Type = "chat" },
        ["ERNIE-3.5-8K-Preview"] = new ErnieBotModelInfo() { Name = "ERNIE-3.5-8K-Preview", Code = "ernie-3.5-8k-preview", Type = "chat" },
        ["ERNIE-3.5-8K-0613"] = new ErnieBotModelInfo() { Name = "ERNIE-3.5-8K-0613", Code = "ernie-3.5-8k-0613", Type = "chat" },
        ["ERNIE-3.5-8K-0329"] = new ErnieBotModelInfo() { Name = "ERNIE-3.5-8K-0329", Code = "ernie-3.5-8k-0329", Type = "chat" },
        ["ERNIE-Speed-128K"] = new ErnieBotModelInfo() { Name = "ERNIE-Speed-128K", Code = "ernie-speed-128k", Type = "chat" },
        ["ERNIE-Speed-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Speed-8K", Code = "ernie_speed", Type = "chat" },
        ["ERNIE Speed-AppBuilder"] = new ErnieBotModelInfo() { Name = "ERNIE Speed-AppBuilder", Code = "ai_apaas", Type = "chat" },
        ["ERNIE-Character-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Character-8K", Code = "ernie-char-8k", Type = "chat" },
        ["ERNIE-Functions-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Functions-8K", Code = "ernie-func-8k", Type = "chat" },
        ["ERNIE-Lite-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Lite-8K", Code = "ernie-lite-8k", Type = "chat" },
        ["ERNIE-Lite-8K-0922"] = new ErnieBotModelInfo() { Name = "ERNIE-Lite-8K-0922", Code = "eb-instant", Type = "chat" },
        ["ERNIE-Lite-AppBuilder-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Lite-AppBuilder-8K", Code = "ai_apaas_lite", Type = "chat" },
        ["ERNIE-Tiny-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Tiny-8K", Code = "ernie-tiny-8k", Type = "chat" },
        ["Qianfan-Chinese-Llama-2-7B"] = new ErnieBotModelInfo() { Name = "Qianfan-Chinese-Llama-2-7B", Code = "qianfan_chinese_llama_2_7b", Type = "chat" },
        ["Qianfan-Chinese-Llama-2-13B"] = new ErnieBotModelInfo() { Name = "Qianfan-Chinese-Llama-2-13B", Code = "qianfan_chinese_llama_2_13b", Type = "chat" },
        ["Qianfan-Chinese-Llama-2-70B"] = new ErnieBotModelInfo() { Name = "Qianfan-Chinese-Llama-2-70B", Code = "qianfan_chinese_llama_2_70b", Type = "chat" },
        ["Meta-Llama-3-8B"] = new ErnieBotModelInfo() { Name = "Meta-Llama-3-8B", Code = "llama_3_8b", Type = "chat" },
        ["Meta-Llama-3-70B"] = new ErnieBotModelInfo() { Name = "Meta-Llama-3-70B", Code = "llama_3_70b", Type = "chat" },
        ["Llama-2-7B-Chat"] = new ErnieBotModelInfo() { Name = "Llama-2-7B-Chat", Code = "llama_2_7b", Type = "chat" },
        ["Llama-2-13B-Chat"] = new ErnieBotModelInfo() { Name = "Llama-2-13B-Chat", Code = "llama_2_13b", Type = "chat" },
        ["Llama-2-70B-Chat"] = new ErnieBotModelInfo() { Name = "Llama-2-70B-Chat", Code = "llama_2_70b", Type = "chat" },
        ["ChatGLM2-6B-32K"] = new ErnieBotModelInfo() { Name = "ChatGLM2-6B-32K", Code = "chatglm2_6b_32k", Type = "chat" },
        ["XuanYuan-70B-Chat-4bit"] = new ErnieBotModelInfo() { Name = "XuanYuan-70B-Chat-4bit", Code = "xuanyuan_70b_chat", Type = "chat" },
        ["Gemma-7B-It"] = new ErnieBotModelInfo() { Name = "Gemma-7B-It", Code = "gemma_7b_it", Type = "chat" },
        ["Yi-34B-Chat"] = new ErnieBotModelInfo() { Name = "Yi-34B-Chat", Code = "yi_34b_chat", Type = "chat" },
        ["Mixtral-8x7B-Instruct"] = new ErnieBotModelInfo() { Name = "Mixtral-8x7B-Instruct", Code = "mixtral_8x7b_instruct", Type = "chat" },
        ["ChatLaw"] = new ErnieBotModelInfo() { Name = "ChatLaw", Code = "chatlaw", Type = "chat" },
        ["Qianfan-BLOOMZ-7B-compressed"] = new ErnieBotModelInfo() { Name = "Qianfan-BLOOMZ-7B-compressed", Code = "qianfan_bloomz_7b_compressed", Type = "chat" },
        ["BLOOMZ-7B"] = new ErnieBotModelInfo() { Name = "BLOOMZ-7B", Code = "bloomz_7b1", Type = "chat" },
        ["AquilaChat-7B"] = new ErnieBotModelInfo() { Name = "AquilaChat-7B", Code = "aquilachat_7b", Type = "chat" },
        ["ERNIE-Character-Fiction-8K"] = new ErnieBotModelInfo() { Name = "ERNIE-Character-Fiction-8K", Code = "ernie-char-fiction-8k", Type = "chat" },
        ["ERNIE-4.0-Turbo-8K-Preview"] = new ErnieBotModelInfo() { Name = "ERNIE-4.0-Turbo-8K-Preview", Code = "ernie-4.0-turbo-8k-preview", Type = "chat" },
        ["bge-large-en"] = new ErnieBotModelInfo() { Name = "bge-large-en", Code = "bge_large_en", Type = "embeddings" },
        ["bge-large-zh"] = new ErnieBotModelInfo() { Name = "bge-large-zh", Code = "bge_large_zh", Type = "embeddings" },
        ["tao-8k"] = new ErnieBotModelInfo() { Name = "tao-8k", Code = "tao_8k", Type = "embeddings" },
        ["Embedding-V1"] = new ErnieBotModelInfo() { Name = "Embedding-V1", Code = "embedding-v1", Type = "embeddings" },
    };

    /// <summary>
    /// 获取模型端点
    /// </summary>
    /// <param name="modelId">模型id</param>
    /// <param name="modelType">模型类型，值有 chat,embeddings</param>
    /// <returns></returns>
    public static string GetModelEndpoint(string modelId, string modelType = "chat")
    {
        modelId = modelId ?? string.Empty;

        if (ModeInfoDict.ContainsKey(modelId))
        {
            var info = ModeInfoDict[modelId];
            if (info.Type == modelType)
            {
                return info.Code;
            }

        }

        return modelId.ToLower().Replace("-", "_");
    }
}