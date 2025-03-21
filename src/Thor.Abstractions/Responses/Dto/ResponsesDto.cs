
using System.Text.Json.Serialization;

namespace Thor.Abstractions.Responses.Dto;

/// <summary>
/// Responses data transfer object.
/// 响应数据传输对象。
/// </summary>
public class ResponsesDto
{
    /// <summary>
    /// Unique identifier for the response.
    /// 响应的唯一标识符。
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    /// <summary>
    /// Object type.
    /// 对象类型。
    /// </summary>
    [JsonPropertyName("object")]
    public string? Object { get; set; }
    
    /// <summary>
    /// Unix timestamp when the response was created.
    /// 创建响应的Unix时间戳。
    /// </summary>
    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }
    
    /// <summary>
    /// Status of the response.
    /// 响应状态。
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    
    /// <summary>
    /// Error information if any.
    /// 错误信息（如果有）。
    /// </summary>
    [JsonPropertyName("error")]
    public object? Error { get; set; }
    
    /// <summary>
    /// Details about incomplete response if applicable.
    /// 不完整响应的详细信息（如适用）。
    /// </summary>
    [JsonPropertyName("incomplete_details")]
    public object? IncompleteDetails { get; set; }
    
    /// <summary>
    /// Instructions for the model.
    /// 模型的指令。
    /// </summary>
    [JsonPropertyName("instructions")]
    public object? Instructions { get; set; }
    
    /// <summary>
    /// Maximum number of output tokens.
    /// 最大输出标记数。
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public object? MaxOutputTokens { get; set; }
    
    /// <summary>
    /// Model used for the response.
    /// 用于响应的模型。
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    /// <summary>
    /// Array of output elements.
    /// 输出元素数组。
    /// </summary>
    [JsonPropertyName("output")]
    public Output[]? Output { get; set; }
    
    /// <summary>
    /// Indicates if tool calls can be executed in parallel.
    /// 指示工具调用是否可以并行执行。
    /// </summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool ParallelToolCalls { get; set; }
    
    /// <summary>
    /// ID of the previous response.
    /// 前一个响应的ID。
    /// </summary>
    [JsonPropertyName("previous_response_id")]
    public object? PreviousResponseId { get; set; }
    
    /// <summary>
    /// Reasoning information.
    /// 推理信息。
    /// </summary>
    [JsonPropertyName("reasoning")]
    public Reasoning? Reasoning { get; set; }
    
    /// <summary>
    /// Indicates if the response should be stored.
    /// 指示是否应存储响应。
    /// </summary>
    [JsonPropertyName("store")]
    public bool Store { get; set; }
    
    /// <summary>
    /// Temperature parameter for response generation.
    /// 响应生成的温度参数。
    /// </summary>
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    /// <summary>
    /// Text configuration.
    /// 文本配置。
    /// </summary>
    [JsonPropertyName("text")]
    public Text? Text { get; set; }
    
    /// <summary>
    /// Tool choice configuration.
    /// 工具选择配置。
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public string? ToolChoice { get; set; }
    
    /// <summary>
    /// Available tools.
    /// 可用的工具。
    /// </summary>
    [JsonPropertyName("tools")]
    public object[]? Tools { get; set; }
    
    /// <summary>
    /// Top-p sampling parameter.
    /// 顶部p采样参数。
    /// </summary>
    [JsonPropertyName("top_p")]
    public double TopP { get; set; }
    
    /// <summary>
    /// Truncation method.
    /// 截断方法。
    /// </summary>
    [JsonPropertyName("truncation")]
    public string? Truncation { get; set; }
    
    /// <summary>
    /// Token usage information.
    /// 令牌使用信息。
    /// </summary>
    [JsonPropertyName("usage")]
    public Usage? Usage { get; set; }
    
    /// <summary>
    /// User identifier.
    /// 用户标识符。
    /// </summary>
    [JsonPropertyName("user")]
    public object? User { get; set; }
    
    /// <summary>
    /// Metadata associated with the response.
    /// 与响应相关的元数据。
    /// </summary>
    [JsonPropertyName("metadata")]
    public object? Metadata { get; set; }
}

/// <summary>
/// Output element of the response.
/// 响应的输出元素。
/// </summary>
public class Output
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
    public Content[]? Content { get; set; }
}

/// <summary>
/// Content element of the output.
/// 输出的内容元素。
/// </summary>
public class Content
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

/// <summary>
/// Reasoning information for the response.
/// 响应的推理信息。
/// </summary>
public class Reasoning
{
    /// <summary>
    /// Effort level for reasoning.
    /// 推理的努力级别。
    /// </summary>
    [JsonPropertyName("effort")]
    public string? Effort { get; set; }
    
    /// <summary>
    /// Summary of reasoning.
    /// 推理摘要。
    /// </summary>
    [JsonPropertyName("summary")]
    public object? Summary { get; set; }
}

/// <summary>
/// Text configuration for the response.
/// 响应的文本配置。
/// </summary>
public class Text
{
    /// <summary>
    /// Format configuration for text.
    /// 文本的格式配置。
    /// </summary>
    [JsonPropertyName("format")]
    public Format? Format { get; set; }
}

/// <summary>
/// Format configuration for text.
/// 文本的格式配置。
/// </summary>
public class Format
{
    /// <summary>
    /// Type of format.
    /// 格式类型。
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

/// <summary>
/// Token usage information.
/// 令牌使用信息。
/// </summary>
public class Usage
{
    /// <summary>
    /// Number of input tokens.
    /// 输入令牌数量。
    /// </summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }
    
    /// <summary>
    /// Details about input tokens.
    /// 有关输入令牌的详细信息。
    /// </summary>
    [JsonPropertyName("input_tokens_details")]
    public InputTokensDetails? InputTokensDetails { get; set; }
    
    /// <summary>
    /// Number of output tokens.
    /// 输出令牌数量。
    /// </summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
    
    /// <summary>
    /// Details about output tokens.
    /// 有关输出令牌的详细信息。
    /// </summary>
    [JsonPropertyName("output_tokens_details")]
    public OutputTokensDetails? OutputTokensDetails { get; set; }
    
    /// <summary>
    /// Total number of tokens.
    /// 令牌总数。
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

/// <summary>
/// Details about input tokens.
/// 有关输入令牌的详细信息。
/// </summary>
public class InputTokensDetails
{
    /// <summary>
    /// Number of cached tokens.
    /// 缓存的令牌数量。
    /// </summary>
    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; set; }
}

/// <summary>
/// Details about output tokens.
/// 有关输出令牌的详细信息。
/// </summary>
public class OutputTokensDetails
{
    /// <summary>
    /// Number of tokens used for reasoning.
    /// 用于推理的令牌数量。
    /// </summary>
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }
}
