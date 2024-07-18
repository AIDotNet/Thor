using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelListResponse : ThorBaseResponse
{
    [JsonPropertyName("data")] public List<ModelResponse> Models { get; set; }
}