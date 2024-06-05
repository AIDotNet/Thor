using System.Collections.Concurrent;
using AIDotNet.Abstractions;
using Azure;
using Azure.AI.OpenAI;

namespace AIDotNet.AzureOpenAI;

public static class AzureOpenAIFactory
{
    private static ConcurrentDictionary<string, OpenAIClient> _clients = new();

    public static OpenAIClient CreateClient(ChatOptions options)
    {
        var key = $"{options.Key}_{options.Address}_{options.Other}";
        return _clients.GetOrAdd(key, (_) =>
        {
            var version = OpenAIClientOptions.ServiceVersion.V2024_04_01_Preview;

            switch (options.Other)
            {
                case "2022-12-01":
                    version = OpenAIClientOptions.ServiceVersion.V2022_12_01;
                    break;
                case "2023-05-15":
                    version = OpenAIClientOptions.ServiceVersion.V2023_05_15;
                    break;
                case "2023-06-01-preview":
                    version = OpenAIClientOptions.ServiceVersion.V2023_06_01_Preview;
                    break;
                case "2023-07-01-preview":
                    version = OpenAIClientOptions.ServiceVersion.V2023_07_01_Preview;
                    break;
                case "2024-02-15-preview":
                    version = OpenAIClientOptions.ServiceVersion.V2024_02_15_Preview;
                    break;
                case "2024-03-01-preview":
                    version = OpenAIClientOptions.ServiceVersion.V2024_03_01_Preview;
                    break;
                case "2024-04-01-preview":
                    version = OpenAIClientOptions.ServiceVersion.V2024_04_01_Preview;
                    break;
            }

            var client = new OpenAIClient(new Uri(options.Address), new AzureKeyCredential(options.Key),
                new OpenAIClientOptions(version));

            return client;
        });
    }
}