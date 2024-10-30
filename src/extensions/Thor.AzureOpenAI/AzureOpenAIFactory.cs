using System.ClientModel;
using System.Collections.Concurrent;
using Azure.AI.OpenAI;
using Thor.Abstractions;

namespace Thor.AzureOpenAI;

public static class AzureOpenAIFactory
{
    private const string AddressTemplate = "{0}/openai/deployments/{1}/chat/completions?api-version={2}";
    private static readonly ConcurrentDictionary<string, AzureOpenAIClient> Clients = new();

    public static string GetAddress(ThorPlatformOptions options, string model)
    {
        if (string.IsNullOrEmpty(options.Other))
        {
            options.Other = "2024-08-01-preview";
        }
        
        return string.Format(AddressTemplate, options.Address.TrimEnd('/'), model, options.Other);
    }

    public static AzureOpenAIClient CreateClient(ThorPlatformOptions options)
    {
        return Clients.GetOrAdd($"{options.ApiKey}_{options.Address}_{options.Other}", (_) =>
        {
            const AzureOpenAIClientOptions.ServiceVersion version = AzureOpenAIClientOptions.ServiceVersion.V2024_06_01;

            var client = new AzureOpenAIClient(new Uri(options.Address), new ApiKeyCredential(options.ApiKey),
                new AzureOpenAIClientOptions(version));

            return client;
        });
    }
}