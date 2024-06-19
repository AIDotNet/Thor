using System.Collections.Concurrent;
using ERNIE_Bot.SDK;

namespace Thor.ErnieBot;

public class ErnieBotClientFactory
{
    private static ConcurrentDictionary<string, ERNIEBotClient> clients = new();

    public static ERNIEBotClient CreateClient(string clientId, string clientSecret)
    {
        var key = $"{clientId}_{clientSecret}";

        var client = clients.GetOrAdd(key, _ => new ERNIEBotClient(clientId, clientSecret));

        return client;
    }
}