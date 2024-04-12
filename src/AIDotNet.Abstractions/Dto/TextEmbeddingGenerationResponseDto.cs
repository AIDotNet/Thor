using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.Dto;

public class TextEmbeddingGenerationResponseDto
{
    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("Model")]
    public string Model { get; set; }

    [JsonPropertyName("usage")]
    public OpenAIUsageDto Usage { get; set; }
}