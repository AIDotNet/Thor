using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime.Model.Internal.MarshallTransformations;
using Amazon.Runtime;
using Amazon.Runtime.Documents.Internal.Transform;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Thor.AWSClaude.Chats.Dto;

namespace Thor.AWSClaude.Chats;

public class ChatResponseUnmarshaller : JsonResponseUnmarshaller
{
    private static ChatResponseUnmarshaller _instance = new ChatResponseUnmarshaller();

    /// <summary>
    /// Unmarshaller the response from the service to the response class.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
    {
        using var memoryStream = new MemoryStream();
        context.Stream.CopyTo(memoryStream);
        return JsonSerializer.Deserialize<AwsChatResponse>(memoryStream.ToArray());
    }

    /// <summary>Unmarshaller error response to exception.</summary>
    /// <param name="context"></param>
    /// <param name="innerException"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public override AmazonServiceException UnmarshallException(
        JsonUnmarshallerContext context,
        Exception innerException,
        HttpStatusCode statusCode)
    {
        StreamingUtf8JsonReader reader1 = new StreamingUtf8JsonReader(context.Stream);
        ErrorResponse errorResponse = JsonErrorResponseUnmarshaller.GetInstance().Unmarshall(context, ref reader1);
        errorResponse.InnerException = innerException;
        errorResponse.StatusCode = statusCode;
        using (MemoryStream responseStream = new MemoryStream(context.GetResponseBodyBytes()))
        {
            using (JsonUnmarshallerContext context1 =
                   new JsonUnmarshallerContext((Stream)responseStream, false, context.ResponseData))
            {
                StreamingUtf8JsonReader reader2 = new StreamingUtf8JsonReader((Stream)responseStream);
                if (errorResponse.Code != null && errorResponse.Code.Equals("AccessDeniedException"))
                    return (AmazonServiceException)AccessDeniedExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("InternalServerException"))
                    return (AmazonServiceException)InternalServerExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ModelErrorException"))
                    return (AmazonServiceException)ModelErrorExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ModelNotReadyException"))
                    return (AmazonServiceException)ModelNotReadyExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ModelTimeoutException"))
                    return (AmazonServiceException)ModelTimeoutExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceNotFoundException"))
                    return (AmazonServiceException)ResourceNotFoundExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ServiceUnavailableException"))
                    return (AmazonServiceException)ServiceUnavailableExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null && errorResponse.Code.Equals("ThrottlingException"))
                    return (AmazonServiceException)ThrottlingExceptionUnmarshaller.Instance.Unmarshall(context1,
                        errorResponse, ref reader2);
                if (errorResponse.Code != null)
                {
                    if (errorResponse.Code.Equals("ValidationException"))
                        return (AmazonServiceException)ValidationExceptionUnmarshaller.Instance.Unmarshall(context1,
                            errorResponse, ref reader2);
                }
            }
        }

        return (AmazonServiceException)new AmazonBedrockRuntimeException(errorResponse.Message,
            errorResponse.InnerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId,
            errorResponse.StatusCode);
    }

    internal static ChatResponseUnmarshaller GetInstance()
    {
        return ChatResponseUnmarshaller._instance;
    }

    /// <summary>Gets the singleton.</summary>
    public static ChatResponseUnmarshaller Instance => ChatResponseUnmarshaller._instance;
}