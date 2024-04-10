using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIMessageDto
{
    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }

    [JsonPropertyName("tool_calls")]
    public OpenAIToolCalls[] ToolCalls { get; set; }
}

public class OpenAIToolCalls
{
    public string id { get; set; }
    
    public string type { get; set; }
    
    public OpenAIToolFunction function { get; set; }
}

public class OpenAIToolFunction
{
    public string name { get; set; }
    
    public string arguments { get; set; }
}