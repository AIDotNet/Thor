using System.Text.Json.Serialization;

namespace Thor.Abstractions.Dtos;

/// <summary>
/// 统计信息模型
/// </summary>
public record ThorUsageResponse
{
    /// <summary>
    /// 提示中的令牌数。
    /// </summary>
    [JsonPropertyName("prompt_tokens")] 
    public int? PromptTokens { get; set; }

    /// <summary>
    /// 生成的完成中的令牌数。
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int? CompletionTokens { get; set; }

    /// <summary>
    /// 请求中使用的令牌总数（提示 + 完成）。
    /// </summary>
    [JsonPropertyName("total_tokens")] 
    public int TotalTokens { get; set; }
}