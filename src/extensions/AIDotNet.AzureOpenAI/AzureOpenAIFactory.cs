using System.Collections.Concurrent;
using AIDotNet.Abstractions;
using Azure;
using Azure.AI.OpenAI;
using OpenAI;

namespace AIDotNet.AzureOpenAI;

public static class AzureOpenAIFactory
{
    private static ConcurrentDictionary<string, AzureOpenAIClient> _clients = new();

    public static AzureOpenAIClient CreateClient(ChatOptions options)
    {
        var key = $"{options.Key}_{options.Address}_{options.Other}";
        return _clients.GetOrAdd(key, (_) =>
        {
            var version = AzureOpenAIClientOptions.ServiceVersion.V2024_04_01_Preview;

            switch (options.Other)
            {
                case "2024-05-01-preview":
                    version = AzureOpenAIClientOptions.ServiceVersion.V2024_05_01_Preview;
                    break;
                case "2024_06_01":
                    version = AzureOpenAIClientOptions.ServiceVersion.V2024_06_01;
                    break;
                case "2024-04-01-preview":
                    version = AzureOpenAIClientOptions.ServiceVersion.V2024_04_01_Preview;
                    break;
            }

            var client = new AzureOpenAIClient (new (options.Address), new AzureKeyCredential(options.Key),new AzureOpenAIClientOptions());

            return client;
        });
    }
}