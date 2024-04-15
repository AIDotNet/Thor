using System.Text.Json.Serialization;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.FileResponseModels;

public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
{
    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}