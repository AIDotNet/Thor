using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FileResponseModels;

public record FileUploadResponse : ThorBaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("bytes")] public int Bytes { get; set; }

    [JsonPropertyName("filename")] public string FileName { get; set; }

    [JsonPropertyName("purpose")] public string Purpose { get; set; }

    [JsonPropertyName("created_at")] public int CreatedAt { get; set; }
}