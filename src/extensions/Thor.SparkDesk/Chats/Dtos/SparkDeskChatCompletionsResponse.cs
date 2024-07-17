using System.Text.Json.Serialization;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;

namespace Thor.SparkDesk.Chats.Dtos
{
    /// <summary>
    /// 星火对话补全相应数据
    /// </summary>
    internal class SparkDeskChatCompletionsResponse
    {
        public SparkDeskChatCompletionsResponse()
        {
            this.Created = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        /// 错误码，0表示正常，非0表示出错；详细释义可在接口说明文档最后的错误码说明了解
        /// 参考文档：https://www.xfyun.cn/doc/spark/HTTP%E8%B0%83%E7%94%A8%E6%96%87%E6%A1%A3.html#_4-%E6%8E%A5%E5%8F%A3%E5%93%8D%E5%BA%94
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// 会话是否成功的描述信息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// 会话的唯一id，用于讯飞技术人员查询服务端会话日志使用,出现调用错误时建议留存该字段
        /// </summary>
        [JsonPropertyName("sid")]
        public string SId { get; set; }

        /// <summary>
        /// 创建对话补全时的 Unix 时间戳（以秒为单位）。
        /// </summary>
        [JsonPropertyName("created")]
        public int Created { get; set; }

        /// <summary>
        /// 响应内容choices模块
        /// </summary>
        [JsonPropertyName("choices")]
        public List<SparkDeskChatChoiceResponse>? Choices { get; set; }

        /// <summary>
        /// 完成请求的使用情况统计信息。<br/>
        /// usage.prompt_tokens	历史问题消耗的tokens<br/>
        /// usage.completion_tokens AI回答消耗的tokens<br/>
        /// usage.total_tokens prompt_tokens和completion_tokens的和，也是本次交互一共消耗的tokens
        /// </summary>
        [JsonPropertyName("usage")]
        public ThorUsageResponse? Usage { get; set; }

        /// <summary>
        /// 转换相应模型
        /// </summary>
        /// <param name="stream">是否是流式输出</param>
        /// <param name="model">模型名</param>
        /// <returns></returns>
        internal ThorChatCompletionsResponse ToThorChatCompletionsResponse(bool stream, string model)
        {

            var thorResponse = new ThorChatCompletionsResponse()
            {
                Id = this.SId,
                Choices = this.Choices.Select(x =>
                {
                    // 星火这里 toolcalls返回是对象，openAI定义的是数组
                    List<ThorToolCall> toolCalls = null;
                    if (stream == true)
                    {
                        if (x.Delta?.ToolCalls is not null)
                        {
                            toolCalls = new List<ThorToolCall>() { x.Delta.ToolCalls };
                        }
                    }
                    else
                    {
                        if (x.Message?.ToolCalls is not null)
                        {
                            toolCalls = new List<ThorToolCall>() { x.Message.ToolCalls };
                        }
                    }
                    
                    return new ThorChatChoiceResponse()
                    {
                        Index = x.Index,
                        Message = ThorChatMessage.CreateAssistantMessage(
                            stream ? x.Delta.Content : x.Message.Content, null, toolCalls),
                    };
                }).ToList(),
                Usage = this.Usage,
                ObjectTypeName = stream ? "chat.completion.chunk" : "chat.completion",
                Model = model,
                Created = this.Created
            };


            return thorResponse;
        }
    }
}
