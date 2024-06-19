using System.Text.Json.Serialization;

namespace Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;

public record FineTuneCancelRequest
{
    [JsonPropertyName("fine_tune_id")] public string FineTuneId { get; set; }
}