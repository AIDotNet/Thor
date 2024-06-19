using System.Text.Json.Serialization;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FileResponseModels;

public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
{
    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}