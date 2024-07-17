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
    /// 对话补全消息体
    /// </summary>
    internal class SparkDeskChatCompletionsMessage
    {
        /// <summary>
        /// 【必填】发出消息的角色，请使用<see cref="ThorChatMessageRoleConst.User"/>赋值,如：ThorChatMessageRoleConst.User
        /// 取值为[system,user,assistant]	
        /// system用于设置对话背景，
        /// user表示是用户的问题，
        /// assistant表示AI的回复
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; }

        /// <summary>
        /// 所有content的累计tokens需控制8192以内
        /// 用户和AI的对话内容,如：你好
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// 工具调用列表，模型生成的工具调用，例如函数调用。
        /// </summary>
        [JsonPropertyName("tool_calls")]
        public ThorToolCall? ToolCalls { get; set; }
    }
}
