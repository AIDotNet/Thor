using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

public record EmbeddingCreateResponse : ThorBaseResponse
{
    [JsonPropertyName("model")] 
    public string Model { get; set; }

    [JsonPropertyName("data")] 
    public List<EmbeddingResponse> Data { get; set; }

    [JsonPropertyName("usage")] 
    public ThorUsageResponse Usage { get; set; }
}

public record EmbeddingResponse
{
    [JsonPropertyName("index")] 
    public int? Index { get; set; }

    [JsonPropertyName("embedding")] 
    public object Embedding { get; set; }
}