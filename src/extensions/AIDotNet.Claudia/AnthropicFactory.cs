using System.Collections.Concurrent;
using Claudia;

namespace AIDotNet.Claudia;

public static class AnthropicFactory
{
    private static ConcurrentDictionary<string, Anthropic> _clients = new();

    public static Anthropic CreateClient(string apiKey, string address)
    {
        var key = $"{apiKey}_{address}";
        return _clients.GetOrAdd(key, (_) =>
        {
            var anthropic = new Anthropic
            {
                ApiKey = apiKey,
            };
            if (!string.IsNullOrEmpty(address))
            {
                anthropic.HttpClient.BaseAddress = new Uri(address);
            }

            return anthropic;
        });
    }
}