using System.Collections.Concurrent;
using Thor.Abstractions;
using Azure;
using Azure.AI.OpenAI;
using OpenAI;

namespace Thor.AzureOpenAI;

public static class AzureOpenAIFactory
{
    private const string AddressTemplate = "{0}/openai/deployments/{1}/chat/completions?api-version={2}";
    private static readonly ConcurrentDictionary<string, AzureOpenAIClient> Clients = new();

    public static string GetAddress(ChatPlatformOptions options, string model)
    {
        return string.Format(AddressTemplate, options.Address.TrimEnd('/'), model, options.Other);
    }

    public static AzureOpenAIClient CreateClient(ChatPlatformOptions options)
    {
        var key = $"{options.ApiKey}_{options.Address}_{options.Other}";
        return Clients.GetOrAdd(key, (_) =>
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

            var client = new AzureOpenAIClient(new(options.Address), new AzureKeyCredential(options.ApiKey),
                new AzureOpenAIClientOptions(version));

            return client;
        });
    }
}