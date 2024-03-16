using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIChatCompletionInput<T> : OpenAICompletionInput
{
    [JsonPropertyName("messages")]
    public List<T> Messages { get; set; }
}