using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

public record ChatChoiceResponse
{
    [JsonPropertyName("delta")]
    public ChatMessage Delta
    {
        get => Message;
        set => Message = value;
    }

    [JsonPropertyName("message")] public ChatMessage Message { get; set; }

    [JsonPropertyName("index")] public int? Index { get; set; }

    [JsonPropertyName("finish_reason")] public string FinishReason { get; set; } = string.Empty;

    [JsonPropertyName("finish_details")] public FinishDetailsResponse? FinishDetails { get; set; }
    public class FinishDetailsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("stop")]
        public string Stop { get; set; }
    }
}