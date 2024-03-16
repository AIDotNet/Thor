using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAICompletionInput
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; } = 0.7f;

    /// <summary>
    /// 用温度采样的另一种方法称为核采样，其中模型考虑具有top_p概率质量的token的结果。因此0.1意味着只考虑包含前10%概率质量的标记。我们通常建议改变这个或“温度”，但不建议两者都改变。
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = true;

    /// <summary>
    /// 生成的答案允许的最大标记数。默认情况下，模型可以返回的token数量为(4096 -提示token)。
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; } = 2048;

    /// <summary>
    /// 在-2.0到2.0之间的数字。正值会根据新标记在文本中存在的频率来惩罚它们，降低模型逐字重复同一行的可能性。[有关频率和存在惩罚的更多信息。](https://docs.api-reference/parameter -details)
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public double? FrequencyPenalty { get; set; }

    [JsonPropertyName("error")]
    public OpenAIErrorDto Error { get; set; }
}