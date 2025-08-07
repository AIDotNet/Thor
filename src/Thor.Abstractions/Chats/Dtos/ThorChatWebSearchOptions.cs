using System.Text.Json.Serialization;

namespace Thor.Abstractions.Chats.Dtos;

public class ThorChatWebSearchOptions
{
    [JsonPropertyName("search_context_size")]
    public string? SearchContextSize { get; set; }
    
    [JsonPropertyName("user_location")]
    public ThorUserLocation? UserLocation { get; set; }
}

public sealed class ThorUserLocation
{
    [JsonPropertyName("type")] public required string Type { get; set; }
    
    [JsonPropertyName("approximate")]
    public ThorUserLocationApproximate? Approximate { get; set; }
}

public sealed class ThorUserLocationApproximate
{
    [JsonPropertyName("city")]
    public string? City { get; set; }
    
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    
    [JsonPropertyName("region")]
    public string? Region { get; set; }
    
    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }
}