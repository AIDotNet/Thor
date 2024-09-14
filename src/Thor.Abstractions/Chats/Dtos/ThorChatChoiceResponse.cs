using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 聊天完成选项列
/// </summary>
public record ThorChatChoiceResponse
{
    /// <summary>
    /// 模型生成的聊天完成消息。【流式】模型响应生成的聊天完成增量存储在此属性。<br/>
    /// 在当前模型中，无论流式还是非流式，Message 和 Delta存储相同的值
    /// </summary>
    [JsonPropertyName("delta")]
    public ThorChatMessage Delta
    {
        get => Message;
        set => Message = value;
    }

    /// <summary>
    /// 模型生成的聊天完成消息。【非流式】返回的消息存储在此属性。<br/>
    /// 在当前模型中，无论流式还是非流式，Message 和 Delta存储相同的值
    /// </summary>
    [JsonPropertyName("message")]
    public ThorChatMessage Message { get; set; }

    /// <summary>
    /// 选项列表中选项的索引。
    /// </summary>
    [JsonPropertyName("index")]
    public int? Index { get; set; }

    /// <summary>
    /// 用于处理请求的服务层。仅当在请求中指定了 service_tier 参数时，才包含此字段。
    /// </summary>
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }

    /// <summary>
    /// 模型停止生成令牌的原因。 
    /// stop 如果模型达到自然停止点或提供的停止序列， 
    /// length 如果达到请求中指定的最大标记数， 
    /// content_filter 如果由于内容过滤器中的标志而省略了内容， 
    /// tool_calls 如果模型调用了工具，或者 function_call （已弃用）
    /// 如果模型调用了函数，则会出现这种情况。
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }

    /// <summary>
    /// 此指纹表示模型运行时使用的后端配置。
    /// 可以与 seed 请求参数结合使用，以了解何时进行了可能影响确定性的后端更改。
    /// </summary>
    [JsonPropertyName("finish_details")]
    public FinishDetailsResponse? FinishDetails { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public class FinishDetailsResponse
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("stop")] public string Stop { get; set; }
    }
}