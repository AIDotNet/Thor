using System.Text.Json.Serialization;

namespace Thor.Abstractions.Realtime.Dto;

public class RealtimeResult
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("event_id")]
    public string EventId { get; set; }
    
    [JsonPropertyName("session")]
    public Session Session { get; set; }
}

public class Session
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("object")]
    public string Object { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("expires_at")]
    public int ExpiresAt { get; set; }
    
    [JsonPropertyName("modalities")]
    public string[] Modalities { get; set; }
    
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }
    
    [JsonPropertyName("voice")]
    public string Voice { get; set; }
    
    [JsonPropertyName("turn_detection")]
    public Turn_detection TurnDetection { get; set; }
    
    [JsonPropertyName("input_audio_format")]
    public string InputAudioFormat { get; set; }
    
    [JsonPropertyName("output_audio_format")]
    public string OutputAudioFormat { get; set; }
    
    [JsonPropertyName("input_audio_transcription")]
    public object InputAudioTranscription { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public string ToolChoice { get; set; }
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("max_response_output_tokens")]
    public string MaxResponseOutputTokens { get; set; }
    
    [JsonPropertyName("tools")]
    public object[] Tools { get; set; }
}

public class Turn_detection
{
    public string type { get; set; }
    public double threshold { get; set; }
    public int prefix_padding_ms { get; set; }
    public int silence_duration_ms { get; set; }
}

