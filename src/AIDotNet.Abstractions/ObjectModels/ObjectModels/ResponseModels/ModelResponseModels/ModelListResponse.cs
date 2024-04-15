using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelListResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<ModelResponse> Models { get; set; }
}