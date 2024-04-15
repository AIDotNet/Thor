using System.Text.Json.Serialization;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace AIDotNet.Abstractions.Dto;

public class CompletionCreateResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public List<ChatChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")]
    public UsageResponse Usage { get; set; }

    [JsonPropertyName("created")]
    public int CreatedAt { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerPrint { get; set; }
}