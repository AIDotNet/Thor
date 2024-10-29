using System.Collections.Concurrent;
using System.Net;
using System.Net.Security;
using Claudia;

namespace Thor.Claudia;

public static class ClaudiaFactory
{
    private static readonly ConcurrentDictionary<string, Anthropic> Clients = new();

    public static Anthropic CreateClient(string apiKey, string address)
    {
        var key = $"{apiKey}_{address}";
        return Clients.GetOrAdd(key, (_) =>
        {
            Anthropic anthropic;


            if (!string.IsNullOrWhiteSpace(address))
            {
                anthropic = new Anthropic(new ClaudiaClientHandler(address)
                {
                    MaxConnectionsPerServer = 300,
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                })
                {
                    ApiKey = apiKey
                };
            }
            else
            {
                anthropic = new Anthropic(new SocketsHttpHandler()
                {
                    MaxConnectionsPerServer = 300,
                    SslOptions = new SslClientAuthenticationOptions()
                    {
                        RemoteCertificateValidationCallback = (_, _, _, _) => true
                    }
                })
                {
                    ApiKey = apiKey
                };
            }

            return anthropic;
        });
    }
}