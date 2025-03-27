using System.Text.Json.Serialization;

namespace Thor.Abstractions.Anthropic;

public class AnthropicCacheControl
{
    [JsonPropertyName("type")]
    public string Type { get; set; } 
}