using System.Text.Json.Serialization;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

public record EditCreateResponse : BaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("choices")] public List<ChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")] public UsageResponse Usage { get; set; }

    [JsonPropertyName("created")] public int CreatedAt { get; set; }
}