using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIChatCompletionRequestInput
{
    public OpenAIChatCompletionRequestInput(string role, string content)
    {
        Role = role;
        Content = content;
    }

    public OpenAIChatCompletionRequestInput()
    {
        
    }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("Name")] public string? Name { get; set; }
}