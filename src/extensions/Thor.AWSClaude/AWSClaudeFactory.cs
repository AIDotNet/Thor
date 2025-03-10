using System.Collections.Concurrent;
using Amazon;
using Amazon.BedrockRuntime;
using Thor.AWSClaude.Chats;

namespace Thor.AWSClaude;

public static class AwsClaudeFactory
{
    private static readonly ConcurrentDictionary<string, AwsAmazonBedrockRuntimeClient> Clients = new();

    public static AwsAmazonBedrockRuntimeClient CreateClient(string awsAccessKeyId, string awsSecretAccessKey,
        RegionEndpoint region)
    {
        return Clients.GetOrAdd($"{awsAccessKeyId}_{awsSecretAccessKey}_{region.SystemName}",
            _ => new AwsAmazonBedrockRuntimeClient(awsAccessKeyId, awsSecretAccessKey,
                region));
    }
}