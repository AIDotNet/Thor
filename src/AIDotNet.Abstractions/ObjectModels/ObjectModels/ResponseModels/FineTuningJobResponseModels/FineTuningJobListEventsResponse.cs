using System.Text.Json.Serialization;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.FineTuningJobResponseModels;

public record FineTuningJobListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<EventResponse> Data { get; set; }
    
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}

