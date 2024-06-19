using System.Text.Json.Serialization;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelListResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<ModelResponse> Models { get; set; }
}