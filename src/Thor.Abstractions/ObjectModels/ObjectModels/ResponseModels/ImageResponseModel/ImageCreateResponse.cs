using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;

public record ImageCreateResponse : ThorBaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("data")] public List<ImageDataResult> Results { get; set; }

    [JsonPropertyName("usage")] public ThorUsageResponse? Usage { get; set; } = new();
    


    public record ImageDataResult
    {
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("b64_json")] public string B64 { get; set; }
        [JsonPropertyName("revised_prompt")] public string RevisedPrompt { get; set; }
    }
}