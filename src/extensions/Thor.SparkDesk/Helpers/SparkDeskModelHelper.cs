using Thor.Abstractions.Consts;
using Thor.Abstractions.Dtos;

namespace Thor.SparkDesk.Helpers
{
    /// <summary>
    /// 模型帮助类
    /// </summary>
    public class SparkDeskModelHelper
    {
        /// <summary>
        /// 模型信息字典,key：模型编码，value：模型信息
        /// </summary>
        public static Dictionary<string, ThorModelInfo> ModeInfoDict = new()
        {
            ["general"] = new ThorModelInfo() { Name = "SparkDesk-Lite", Code = "general", Type = "chat" },
            //generalv2 不可用，报错
            ["generalv3"] = new ThorModelInfo() { Name = "SparkDesk-Pro", Code = "generalv3", Type = "chat" },
            ["generalv3.5"] = new ThorModelInfo() { Name = "SparkDesk-Max", Code = "generalv3.5", Type = "chat" },
            ["4.0Ultra"] = new ThorModelInfo() { Name = "SparkDesk-Ultra", Code = "4.0Ultra", Type = "chat" },
        };

        /// <summary>
        /// 模型名称映射，兼容现有名称
        /// </summary>
        public static Dictionary<string, string> ModelNameMap = new()
        {
            ["SparkDesk-Lite"] = "general",
            ["SparkDesk-v1.5"] = "general",

            ["SparkDesk-Pro"] = "generalv3",
            ["SparkDesk-v3.1"] = "generalv3",

            ["SparkDesk-Max"] = "generalv3.5",
            ["SparkDesk-v3.5"] = "generalv3.5",

            ["SparkDesk-Ultra"] = "4.0Ultra",
            ["general-4.0-ultra"] = "4.0Ultra",

        };

        /// <summary>
        /// 获取模型编码
        /// </summary>
        /// <param name="modelId">模型id</param>
        /// <param name="modelType">模型类型，值有 chat,embeddings 等等，使用<see cref="ThorModelTypeConst"/> 赋值</param>
        /// <returns></returns>
        public static string GetModelCode(string modelId, string modelType = "chat")
        {
            modelId = modelId ?? string.Empty;

            if (ModelNameMap.ContainsKey(modelId))
            {
                modelId = ModelNameMap[modelId];
            }

            if (ModeInfoDict.ContainsKey(modelId))
            {
                var info = ModeInfoDict[modelId];
                if (info.Type == modelType)
                {
                    return info.Code;
                }

            }

            return modelId.ToLower();
        }
    }
}
