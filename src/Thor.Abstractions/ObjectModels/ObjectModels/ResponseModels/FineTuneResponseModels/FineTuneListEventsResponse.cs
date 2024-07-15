using System.Text.Json.Serialization;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : ThorBaseResponse
{
    [JsonPropertyName("data")] public List<EventResponse> Data { get; set; }
}