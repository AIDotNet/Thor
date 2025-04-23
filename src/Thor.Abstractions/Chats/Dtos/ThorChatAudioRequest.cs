using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

public sealed class ThorChatAudioRequest
{
    [JsonPropertyName("voice")]
    public string? Voice { get; set; }
    
    [JsonPropertyName("format")]
    public string? Format { get; set; }
}