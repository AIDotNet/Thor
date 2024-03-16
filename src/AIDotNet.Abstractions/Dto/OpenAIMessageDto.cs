using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIMessageDto
{
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
}