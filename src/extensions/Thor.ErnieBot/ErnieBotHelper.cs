using ERNIE_Bot.SDK;

namespace Thor.ErnieBot;

public class ErnieBotHelper
{
    public static string GetModelEndpoint(string modelId)
    {
        if (modelId.Equals("ERNIE-Bot", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot;

        if (modelId.Equals("ERNIE-Bot-4", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot;

        if (modelId.Equals("ERNIE-Bot-8K", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot_8K;

        if (modelId.Equals("ERNIE-Bot-Turbo", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot_Turbo;

        if (modelId.Equals("ERNIE-Bot-Speed", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot_Speed;

        if (modelId.Equals("ERNIE-Bot-35-4K-0205", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot_35_4K_0205;

        if (modelId.Equals("ERNIE-Bot-35-8K-0205", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ERNIE_Bot_35_8K_0205;

        if (modelId.Equals("bge-large-en", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.bge_large_en;

        if (modelId.Equals("bge-large-zh", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.bge_large_zh;

        if (modelId.Equals("Embedding-v1", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Embedding_v1;

        if (modelId.Equals("Llama-2-7b-chat", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Llama_2_7b_chat;

        if (modelId.Equals("Llama-2-13b-chat", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Llama_2_13b_chat;

        if (modelId.Equals("Llama-2-70b-chat", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Llama_2_70b_chat;

        if (modelId.Equals("tao-8k", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Tao_8k;

        if (modelId.Equals("AquilaChat-7b", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.AquilaChat_7b;

        if (modelId.Equals("ChatLaw", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ChatLaw;

        if (modelId.Equals("Qianfan-Chinese-Llama-2-7b", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Qianfan_Chinese_Llama_2_7b;

        if (modelId.Equals("Qianfan-Chinese-Llama-2-13b", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Qianfan_Chinese_Llama_2_13b;

        if (modelId.Equals("ChatGLM2-6b-32k", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.ChatGLM2_6b_32k;

        if (modelId.Equals("XuanYuan-70B-Chat-4bit", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.XuanYuan_70B_Chat_4bit;

        if (modelId.Equals("BLOOMZ-7B", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.BLOOMZ_7B;

        if (modelId.Equals("Qianfan-BLOOMZ-7B-compressed", StringComparison.OrdinalIgnoreCase))
            return ModelEndpoints.Qianfan_BLOOMZ_7B_compressed;

        return modelId;
    }
}