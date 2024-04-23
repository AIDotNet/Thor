using System.Text.Json;
using System.Text.Json.Serialization;

namespace AIDotNet.Abstractions;

public static class AIDtoNetJsonSerializer
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
}