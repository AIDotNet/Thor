using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIFunctionInput
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("function")]
    public OpenAIFunctionFunction Function { get; set; }
}

public class OpenAIFunctionFunction
{
    public string name { get; set; }
    public string description { get; set; }
    public OpenAIFunctionParameters parameters { get; set; }
}

public class OpenAIFunctionParameters
{
    public string type { get; set; }
    public OpenAIFunctionProperties properties { get; set; }
    public string[] required { get; set; }
}

public class OpenAIFunctionProperties
{
    public OpenAIFunctionLocation location { get; set; }
    public OpenAIFunctionUnit unit { get; set; }
}

public class OpenAIFunctionLocation
{
    public string type { get; set; }
    public string description { get; set; }
}

public class OpenAIFunctionUnit
{
    public string type { get; set; }

    [JsonPropertyName("enum")] public string[] Enum { get; set; }
}