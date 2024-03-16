using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class OpenAIResultDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("object")]
    public string _object { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; }
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("choices")]
    public OpenAIChoiceDto[] Choices { get; set; }
    
    [JsonPropertyName("usage")]
    public OpenAIUsageDto Usage { get; set; }

    [JsonPropertyName("error")]
    public OpenAIErrorDto Error { get; set; }
}