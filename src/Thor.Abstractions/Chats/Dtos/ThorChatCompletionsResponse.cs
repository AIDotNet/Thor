using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 对话补全服务返回结果
/// </summary>
public record ThorChatCompletionsResponse
{
    /// <summary>
    /// 对话补全的唯一标识符。
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// 用于对话补全的模型。
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    /// 对象类型<br/>
    /// 非流式对话补全始终为 chat.completion<br/>
    /// 流式对话补全始终为 chat.completion.chunk<br/>
    /// </summary>
    [JsonPropertyName("object")]
    public string? ObjectTypeName { get; set; }

    /// <summary>
    /// 对话补全选项列表。如果 n 大于 1，则可以是多个。
    /// </summary>
    [JsonPropertyName("choices")]
    public List<ThorChatChoiceResponse>? Choices { get; set; }

    /// <summary>
    /// 完成请求的使用情况统计信息。
    /// </summary>
    [JsonPropertyName("usage")]
    public ThorUsageResponse? Usage { get; set; }

    /// <summary>
    /// 创建对话补全时的 Unix 时间戳（以秒为单位）。
    /// </summary>
    [JsonPropertyName("created")]
    public int Created { get; set; }

    /// <summary>
    /// 此指纹表示模型运行时使用的后端配置。
    /// 可以与 seed 请求参数结合使用，以了解何时进行了可能影响确定性的后端更改。
    /// </summary>
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerPrint { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [JsonPropertyName("error")]
    public ThorError? Error { get; set; }
}