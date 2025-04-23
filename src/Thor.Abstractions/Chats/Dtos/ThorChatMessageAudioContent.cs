
using System.Text.Json.Serialization;

public sealed class ThorChatMessageAudioContent
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
    
    [JsonPropertyName("format")]
    public string? Format { get; set; }
}