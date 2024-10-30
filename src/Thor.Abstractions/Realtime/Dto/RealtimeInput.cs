using System.Text.Json.Serialization;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeInput
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("audio")]
    public string? audio { get; set; }
    
    [JsonPropertyName("event_id")] public string EventId { get; set; }
}

public class RealtimeInputItem
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("content")] public List<RealtimeInputItemContent> Content { get; set; }
}

public class RealtimeInputItemContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    public string? Text { get; set; }
    
    [JsonPropertyName("audio")]
    public string? audio { get; set; }
}