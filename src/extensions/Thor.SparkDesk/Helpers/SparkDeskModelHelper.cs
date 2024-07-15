using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.SparkDesk.Helpers.Dtos;

namespace Thor.SparkDesk.Helpers
{
    public class SparkDeskModelHelper
    {
        /// <summary>
        /// 模型信息字典,key：模型编码，value：模型信息
        /// </summary>
        public static Dictionary<string, SparkDeskModelInfo> ModeInfoDict = new()
        {
            ["SparkDesk-V1.1(Lite)"] = new SparkDeskModelInfo() { Name = "SparkDesk-V1.1(Lite)", Code = "general", Type = "chat" },
            ["SparkDesk-V2.1(V2.0)"] = new SparkDeskModelInfo() { Name = "SparkDesk-V2.1(V2.0)", Code = "generalv2", Type = "chat" },
            ["SparkDesk-V3.1(Pro)"] = new SparkDeskModelInfo() { Name = "SparkDesk-V3.1(Pro)", Code = "generalv3", Type = "chat" },
            ["SparkDesk-V3.5(Max)"] = new SparkDeskModelInfo() { Name = "SparkDesk-V3.5(Max)", Code = "generalv3.5", Type = "chat" },
            ["SparkDesk-V4.0(Ultra)"] = new SparkDeskModelInfo() { Name = "SparkDesk-V4.0(Ultra)", Code = "4.0Ultra", Type = "chat" },
        };

        /// <summary>
        /// 获取模型端点
        /// </summary>
        /// <param name="modelId">模型id</param>
        /// <param name="modelType">模型类型，值有 chat,embeddings</param>
        /// <returns></returns>
        public static string GetModelCode(string modelId, string modelType = "chat")
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
}
