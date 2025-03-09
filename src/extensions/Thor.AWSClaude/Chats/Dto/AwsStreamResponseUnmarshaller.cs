using System.Net;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime.Model.Internal.MarshallTransformations;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;

namespace Thor.AWSClaude.Chats;

/// <summary>
/// Response Unmarshaller for ConverseStream operation
/// </summary>  
public class AwsStreamResponseUnmarshaller : JsonResponseUnmarshaller
{
    /// <summary>
    /// Unmarshaller the response from the service to the response class.
    /// </summary>  
    /// <param name="context"></param>
    /// <returns></returns>
    public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
    {
        var response = new AwsWebServiceResponse
        {
            Stream = new AwsStreamOutput(context.Stream)
        };

        return response;
    }

    /// <summary>
    /// Unmarshaller error response to exception.
    /// </summary>  
    /// <param name="context"></param>
    /// <param name="innerException"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public override AmazonServiceException UnmarshallException(JsonUnmarshallerContext context,
        Exception innerException, HttpStatusCode statusCode)
    {
        StreamingUtf8JsonReader reader = new StreamingUtf8JsonReader(context.Stream);
        var errorResponse = JsonErrorResponseUnmarshaller.GetInstance().Unmarshall(context, ref reader);
        errorResponse.InnerException = innerException;
        errorResponse.StatusCode = statusCode;

        var responseBodyBytes = context.GetResponseBodyBytes();

        using (var streamCopy = new MemoryStream(responseBodyBytes))
        using (var contextCopy = new JsonUnmarshallerContext(streamCopy, false, context.ResponseData))
        {
            StreamingUtf8JsonReader readerCopy = new StreamingUtf8JsonReader(streamCopy);
            if (errorResponse.Code is "AccessDeniedException")
            {
                return AccessDeniedExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "InternalServerException")
            {
                return InternalServerExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "ModelErrorException")
            {
                return ModelErrorExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse, ref readerCopy);
            }

            if (errorResponse.Code is "ModelNotReadyException")
            {
                return ModelNotReadyExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "ModelTimeoutException")
            {
                return ModelTimeoutExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "ResourceNotFoundException")
            {
                return ResourceNotFoundExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "ServiceUnavailableException")
            {
                return ServiceUnavailableExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse,
                    ref readerCopy);
            }

            if (errorResponse.Code is "ThrottlingException")
            {
                return ThrottlingExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse, ref readerCopy);
            }

            if (errorResponse.Code is "ValidationException")
            {
                return ValidationExceptionUnmarshaller.Instance.Unmarshall(contextCopy, errorResponse, ref readerCopy);
            }
        }

        return new AmazonBedrockRuntimeException(errorResponse.Message, errorResponse.InnerException,
            errorResponse.Type, errorResponse.Code, errorResponse.RequestId, errorResponse.StatusCode);
    }

    private static AwsStreamResponseUnmarshaller _instance = new AwsStreamResponseUnmarshaller();

    internal static AwsStreamResponseUnmarshaller GetInstance()
    {
        return _instance;
    }

    /// <summary>
    /// Gets the singleton.
    /// </summary>  
    public static AwsStreamResponseUnmarshaller Instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Return false for reading the entire response
    /// </summary>
    /// <param name="response"></param>
    /// <param name="readEntireResponse"></param>
    /// <returns></returns>
    protected override bool ShouldReadEntireResponse(IWebResponseData response, bool readEntireResponse)
    {
        return false;
    }

    /// <summary>
    /// Specifies that the response should be streamed
    /// </summary>
    public override bool HasStreamingProperty => true;
}