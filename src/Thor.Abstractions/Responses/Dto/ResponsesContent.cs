using System.Text.Json.Serialization;

namespace Thor.Abstractions.Responses.Dto;

/// <summary>
/// Content element of the output.
/// 输出的内容元素。
/// </summary>
public class ResponsesContent
{
    /// <summary>
    /// Type of content.
    /// 内容类型。
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Text content.
    /// 文本内容。
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Annotations for the content.
    /// 内容的注释。
    /// </summary>
    [JsonPropertyName("annotations")]
    public object[]? Annotations { get; set; }
}