using System.Text.Json.Serialization;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeItemContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    public string? Text { get; set; }
    
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }
}