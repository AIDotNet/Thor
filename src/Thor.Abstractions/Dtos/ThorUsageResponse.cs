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

    /// <summary>
    /// ThorUsageResponsePromptTokensDetails
    /// </summary>
    [JsonPropertyName("prompt_tokens_details")]
    public ThorUsageResponsePromptTokensDetails? PromptTokensDetails { get; set; }

    /// <summary>
    /// ThorUsageResponseCompletionTokensDetails
    /// </summary>
    [JsonPropertyName("completion_tokens_details")]
    public ThorUsageResponseCompletionTokensDetails? CompletionTokensDetails { get; set; }
}

public record ThorUsageResponsePromptTokensDetails
{
    /// <summary>
    /// 缓存的令牌数。
    /// </summary>
    [JsonPropertyName("cached_tokens")]
    public int? CachedTokens { get; set; }
}

/// <summary>
/// completion_tokens_details
/// </summary>
public record ThorUsageResponseCompletionTokensDetails
{
    /// <summary>
    /// 使用 Predicted Outputs 时， Prediction 的 Final。
    /// </summary>
    [JsonPropertyName("accepted_prediction_tokens")]
    public int? AcceptedPredictionTokens { get; set; }

    /// <summary>
    /// 模型生成的音频输入令牌。
    /// </summary>
    [JsonPropertyName("audio_tokens")]
    public int? AudioTokens { get; set; }

    /// <summary>
    /// 模型生成的用于推理的 Token。
    /// </summary>
    [JsonPropertyName("reasoning_tokens")]
    public int? ReasoningTokens { get; set; }

    /// <summary>
    /// 使用 Predicted Outputs 时， 预测，但未出现在 completion 中。但是，与 reasoning 令牌，这些令牌仍然计入总数 用于 Billing、Output 和 Context Window 的完成令牌 限制。
    /// </summary>
    [JsonPropertyName("rejected_prediction_tokens")]
    public int? RejectedPredictionTokens { get; set; }
}