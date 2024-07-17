using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Thor.Abstractions.Chats.Consts;
using Thor.Abstractions.Chats.Dtos;
using Thor.SparkDesk.Helpers;

namespace Thor.SparkDesk.Chats.Dtos
{
    /// <summary>
    /// 星火对话补全请求参数
    /// </summary>
    internal class SparkDeskChatCompletionsRequest
    {
        /// <summary>
        /// 取值为[general,generalv2,generalv3,generalv3.5,4.0Ultra]
        /// 选择请求的模型版本
        /// general指向Lite版本；
        /// generalv2指向V2.0版本；
        /// generalv3指向Pro版本；
        /// generalv3.5指向Max版本；
        /// 4.0Ultra指向4.0 Ultra版本；
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// 默认False,是否开启流式传输
        /// </summary>
        [JsonPropertyName("stream")]
        public bool? Stream { get; set; }

        /// <summary>
        /// 取值范围 [0,2] ，默认值1	
        /// 核采样阈值。
        /// 用于决定结果随机性，取值越高随机性越强即相同的问题得到的不同答案的可能性越高
        /// </summary>
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        /// <summary>
        /// 可供模型调用的工具
        /// </summary>
        [JsonPropertyName("tools")]
        public List<ThorToolDefinition>? Tools { get; set; }

        /// <summary>
        /// 取值为"none": 不调用，"auto": 自动判断	用于控制模型是如何选择要调用的函数
        /// </summary>
        [JsonPropertyName("tool_choice")]
        public string? ToolChoice { get; set; }

        /// <summary>
        /// 取值为[1,8192]，默认为 4096。模型回答的tokens的最大长度
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }
        /// <summary>
        /// 取值为[1,6],默认为4,从k个候选中随机选择⼀个（⾮等概率）
        /// </summary>
        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        /// <summary>
        /// 包含迄今为止对话的消息列表
        /// </summary>
        [JsonPropertyName("messages")]
        public List<SparkDeskChatCompletionsMessage> Messages { get; set; }

        /// <summary>
        /// 基于 ThorChatCompletionsRequest 转换出请求对象
        /// </summary>
        /// <param name="thorRequest"></param>
        /// <returns></returns>
        public static SparkDeskChatCompletionsRequest CreateByThorChatCompletionsRequest(ThorChatCompletionsRequest thorRequest)
        {
            var model = SparkDeskModelHelper.GetModelCode(thorRequest.Model, "chat");
            //0-1=>1-6 top_k=5*top_p+1
            int? topK = 4;
            if (thorRequest.TopP is not null)
            {
                topK = (int)(Math.Round(thorRequest.TopP.Value * 5 + 1, MidpointRounding.AwayFromZero));
            }

            string? toolChoice = null;
            if (thorRequest.ToolChoice?.Type == ThorToolChoiceTypeConst.Auto)
            {
                toolChoice = ThorToolChoiceTypeConst.Auto;
            }
            else if (thorRequest.ToolChoice?.Type == ThorToolChoiceTypeConst.None)
            {
                toolChoice = ThorToolChoiceTypeConst.None;
            }
            else if (thorRequest.ToolChoice != null)
            {
                toolChoice = JsonSerializer.Serialize(thorRequest.ToolChoice);
            }

            var messages = thorRequest.Messages
                .Select(x => new SparkDeskChatCompletionsMessage()
                {
                    Role = x.Role,
                    Content = x.Content ?? string.Empty
                })
                .ToList();

            var sparkDeskRequest = new SparkDeskChatCompletionsRequest()
            {
                Model = model,
                Stream = thorRequest.Stream,
                Temperature = thorRequest.Temperature,
                TopK = topK,
                MaxTokens = thorRequest.MaxTokens,
                Tools = thorRequest.Tools,
                ToolChoice = toolChoice,
                Messages = messages,

            };

            return sparkDeskRequest;
        }
    }
}
