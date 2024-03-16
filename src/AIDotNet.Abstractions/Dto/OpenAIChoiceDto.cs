using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIChoiceDto
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public OpenAIMessageDto Message { get; set; }

    [JsonPropertyName("delta")]
    public OpenAIMessageDto Delta { get; set; }

    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; } = null;
    
    [JsonPropertyName("logprobs")]
    public string? Logprobs { get; set; } = null;
}