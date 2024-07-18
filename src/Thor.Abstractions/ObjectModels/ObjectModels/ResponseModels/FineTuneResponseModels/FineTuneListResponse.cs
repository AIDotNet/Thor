using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListResponse : ThorBaseResponse
{
    [JsonPropertyName("data")] public List<FineTuneResponse> Data { get; set; }
}