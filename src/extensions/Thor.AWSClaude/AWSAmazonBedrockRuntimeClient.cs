using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime.Model.Internal.MarshallTransformations;
using Amazon.Runtime.Internal;
using Thor.AWSClaude.Chats.Dto;

namespace Thor.AWSClaude.Chats;

public class AwsAmazonBedrockRuntimeClient(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region)
    : AmazonBedrockRuntimeClient(awsAccessKeyId, awsSecretAccessKey, region)
{
    public Task<AwsWebServiceResponse> ChatStreamAsync(ConverseStreamRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var options = new InvokeOptions
        {
            RequestMarshaller = ConverseStreamRequestMarshaller.Instance,
            ResponseUnmarshaller = AwsStreamResponseUnmarshaller.Instance
        };

        return InvokeAsync<AwsWebServiceResponse>(request, options, cancellationToken);
    }

    public virtual Task<AwsChatResponse> ChatAsync(ConverseRequest request,
        System.Threading.CancellationToken cancellationToken = default(CancellationToken))
    {
        var options = new InvokeOptions
        {
            RequestMarshaller = ConverseRequestMarshaller.Instance,
            ResponseUnmarshaller = ChatResponseUnmarshaller.Instance
        };

        return InvokeAsync<AwsChatResponse>(request, options, cancellationToken);
    }
}