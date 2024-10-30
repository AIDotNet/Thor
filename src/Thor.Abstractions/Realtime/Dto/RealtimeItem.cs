using System.Text.Json.Serialization;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string @object { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("content")] 
    public List<RealtimeItemContent> Content { get; set; }
}