using System.Collections.Concurrent;
using System.Net;
using System.Net.Security;
using Claudia;

namespace AIDotNet.Claudia;

public static class AnthropicFactory
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
                anthropic = new Anthropic(new AnthropicClientHandler(address)
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
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
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
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