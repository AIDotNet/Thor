using System.Collections.Concurrent;
using System.Net;
using System.Net.Security;
using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Thor.Abstractions;

namespace Thor.Claudia;

public static class AWSClaudeFactory
{
    private static readonly ConcurrentDictionary<string, AmazonBedrockRuntimeClient> Clients = new();

    public static AmazonBedrockRuntimeClient CreateClient(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region)
    {
        return Clients.GetOrAdd($"{awsAccessKeyId}_{awsSecretAccessKey}_{region.SystemName}", _ => new AmazonBedrockRuntimeClient(awsAccessKeyId, awsSecretAccessKey, region));
    }
}