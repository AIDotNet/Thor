using System.Text.Json.Serialization;

namespace Thor.Abstractions.Responses.Dto;

/// <summary>
/// Output element of the response.
/// 响应的输出元素。
/// </summary>
public class ResponsesOutputDto
{
    /// <summary>
    /// Type of output.
    /// 输出类型。
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Unique identifier for the output.
    /// 输出的唯一标识符。
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Status of the output.
    /// 输出状态。
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Role associated with the output.
    /// 与输出相关联的角色。
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    /// <summary>
    /// Array of content elements.
    /// 内容元素数组。
    /// </summary>
    [JsonPropertyName("content")]
    public ResponsesContent[]? Content { get; set; }

    [JsonPropertyName("queries")] public object[]? Queries { get; set; }

    [JsonPropertyName("results")] public ResponsesOutputFileContentResults[]? Results { get; set; }

    [JsonPropertyName("arguments")] public string? Arguments { get; set; }

    [JsonPropertyName("call_id")] public string? CallId { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("action")] public object? Action { get; set; }

    [JsonPropertyName("pending_safety_checks")]
    public object[]? PendingSafetyChecks { get; set; }

    [JsonPropertyName("summary")] public object[]? Summary { get; set; }

    [JsonPropertyName("encrypted_content")]
    public string? EncryptedContent { get; set; }

    [JsonPropertyName("result")] public string? Result { get; set; }

    [JsonPropertyName("code")] public string? Code { get; set; }

    [JsonPropertyName("server_label")] public string? ServerLabel { get; set; }

    [JsonPropertyName("error")] public string? Error { get; set; }

    [JsonPropertyName("output")] public string? Output { get; set; }
    
    [JsonPropertyName("tools")] public IList<ResponsesToolsDto>? Tools { get; set; }
}

public class ResponsesToolsDto
{
    [JsonPropertyName("input_schema")]
    public object? InputSchema { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("annotations")]
    public object? Annotations { get; set; }
}


public class ResponsesOutputFileContentResults
{
    [JsonPropertyName("attributes")] public Dictionary<string, object>? Attributes { get; set; }

    [JsonPropertyName("file_id")] public string? FileId { get; set; }

    [JsonPropertyName("file_name")] public string? FileName { get; set; }

    //score
    [JsonPropertyName("score")] public double? Score { get; set; }

    [JsonPropertyName("text")] public string? Text { get; set; }

    [JsonPropertyName("logs")] public string? Logs { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("files")] public ResponsesOutputFileContentFile[]? Files { get; set; }
}

public class ResponsesOutputFileContentFile
{
    [JsonPropertyName("file_id")] public string? FileId { get; set; }

    [JsonPropertyName("mime_type")] public string? MimeType { get; set; }
}