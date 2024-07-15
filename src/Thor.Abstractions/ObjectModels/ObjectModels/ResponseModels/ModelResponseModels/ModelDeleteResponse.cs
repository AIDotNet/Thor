using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelDeleteResponse : ThorBaseResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
}