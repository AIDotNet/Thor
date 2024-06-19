using System.Text.Json.Serialization;

namespace Thor.Abstractions.Dto;

public sealed class EmbeddingInput
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("input")]
    public object Input { get; set; }

    [JsonPropertyName("encoding_format")] 
    public string EncodingFormat { get; set; }

    [JsonPropertyName("dimensions")]
    public int? Dimensions { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }
}

