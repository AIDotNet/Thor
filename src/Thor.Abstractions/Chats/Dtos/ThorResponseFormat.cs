using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

/// <summary>
/// 指定模型必须输出的格式的对象。用于启用JSON模式。
/// </summary>
public class ThorResponseFormat
{
    /// <summary>
    /// 设置为json_object启用json模式。
    /// 这保证了模型生成的消息是有效的JSON。
    /// 注意，如果finish_reason=“length”，则消息内容可能是部分的，
    /// 这表示生成超过了max_tokens或对话超过了最大上下文长度。
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("json_schema")]
    public ThorResponseJsonSchema JsonSchema { get; set; }
}