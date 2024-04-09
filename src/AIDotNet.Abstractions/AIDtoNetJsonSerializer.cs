using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions;

public static class AIDtoNetJsonSerializer
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}