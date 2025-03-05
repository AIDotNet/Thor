using System.Text.Json;
using Amazon.BedrockRuntime.Model.Internal.MarshallTransformations;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;

namespace Thor.AWSClaude.Chats;

/// <summary>
/// Response Unmarshaller for ContentBlockDeltaEvent Object
/// </summary>  
public class
    ReasoningContentBlockDeltaEventUnmarshaller : IJsonUnmarshaller<ReasoningContentBlockDeltaEvent,
    JsonUnmarshallerContext>
{
    /// <summary>
    /// Unmarshaller the response from the service to the response class.
    /// </summary>  
    /// <param name="context"></param>
    /// <param name="reader"></param>
    /// <returns>The unmarshalled object</returns>
    public ReasoningContentBlockDeltaEvent Unmarshall(JsonUnmarshallerContext context,
        ref StreamingUtf8JsonReader reader)
    {
        return JsonSerializer.Deserialize<ReasoningContentBlockDeltaEvent>(reader.Reader.ValueSpan);
    }


    private static ReasoningContentBlockDeltaEventUnmarshaller _instance =
        new ReasoningContentBlockDeltaEventUnmarshaller();

    /// <summary>
    /// Gets the singleton.
    /// </summary>  
    public static ReasoningContentBlockDeltaEventUnmarshaller Instance
    {
        get { return _instance; }
    }
}