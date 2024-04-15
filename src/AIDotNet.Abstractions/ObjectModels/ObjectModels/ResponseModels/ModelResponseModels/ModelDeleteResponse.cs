using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelDeleteResponse : BaseResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
}