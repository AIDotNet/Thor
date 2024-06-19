using System.Text.Json.Serialization;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<EventResponse> Data { get; set; }
}