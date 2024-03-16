using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIChatCompletionRequestInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}