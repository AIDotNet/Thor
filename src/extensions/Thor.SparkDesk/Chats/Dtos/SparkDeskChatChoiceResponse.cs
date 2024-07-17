using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.SparkDesk.Chats.Dtos
{
    /// <summary>
    /// 响应内容choices模块
    /// </summary>
    internal class SparkDeskChatChoiceResponse
    {
        /// <summary>
        /// 模型生成的聊天完成消息。【流式】模型响应生成的聊天完成增量存储在此属性。
        /// </summary>
        [JsonPropertyName("delta")]
        public SparkDeskChatCompletionsMessage Delta { get; set; }

        /// <summary>
        /// 模型生成的聊天完成消息。【非流式】返回的消息存储在此属性。
        /// </summary>
        [JsonPropertyName("message")]
        public SparkDeskChatCompletionsMessage Message { get; set; }

        /// <summary>
        /// 结果序号，取值为[0,10]; 当前为保留字段，开发者可忽略
        /// </summary>
        [JsonPropertyName("index")]
        public int? Index { get; set; }
    }
}
