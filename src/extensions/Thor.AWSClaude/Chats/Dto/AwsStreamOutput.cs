using System.Text;
using System.Text.Json;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.BedrockRuntime.Model.Internal.MarshallTransformations;
using Amazon.Runtime.EventStreams;
using Amazon.Runtime.EventStreams.Internal;
using Amazon.Runtime.EventStreams.Utils;
using Amazon.Runtime.Internal.Util;
using Thor.Abstractions;

namespace Thor.AWSClaude.Chats;

public class AwsStreamOutput : EnumerableEventStream<IEventStreamEvent, BedrockRuntimeEventStreamException>
{
    /// <summary>
    ///The mapping of event message to a generator function to construct the matching EventStream event
    /// </summary>
    protected override IDictionary<string, Func<IEventStreamMessage, IEventStreamEvent>> EventMapping { get; } =
        new Dictionary<string, Func<IEventStreamMessage, IEventStreamEvent>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Initial-Response", payload => new InitialResponseEvent(payload) },
            {
                "ContentBlockDelta", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);

                    if (context.Stream is MemoryStream str)
                    {
                        // 转换字符串
                        var strContent = Encoding.UTF8.GetString(str.ToArray());
                        return JsonSerializer.Deserialize<ReasoningContentBlockDeltaEvent>(strContent,ThorJsonSerializer.DefaultOptions);
                    }
                    var reader = new StreamingUtf8JsonReader(context.Stream);

                    return new ReasoningContentBlockDeltaEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
            {
                "ContentBlockStart", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new ContentBlockStartEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
            {
                "ContentBlockStop", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    
                    return new ContentBlockStopEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
            {
                "MessageStart", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new MessageStartEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
            {
                "MessageStop", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new MessageStopEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
            {
                "Metadata", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new ConverseStreamMetadataEventUnmarshaller().Unmarshall(context, ref reader);
                }
            },
        };

    /// <summary>
    /// The mapping of event message to a generator function to construct the matching EventStream Exception
    /// </summary>
    protected override IDictionary<string, Func<IEventStreamMessage, BedrockRuntimeEventStreamException>>
        ExceptionMapping { get; } =
        new Dictionary<string, Func<IEventStreamMessage, BedrockRuntimeEventStreamException>>(StringComparer
            .OrdinalIgnoreCase)
        {
            {
                "InternalServerException", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new BedrockRuntimeEventStreamException(Encoding.UTF8.GetString(payload.Payload),
                        new InternalServerExceptionUnmarshaller().Unmarshall(context, ref reader));
                }
            },
            {
                "ModelStreamErrorException", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new BedrockRuntimeEventStreamException(Encoding.UTF8.GetString(payload.Payload),
                        new ModelStreamErrorExceptionUnmarshaller().Unmarshall(context, ref reader));
                }
            },
            {
                "ServiceUnavailableException", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new BedrockRuntimeEventStreamException(Encoding.UTF8.GetString(payload.Payload),
                        new ServiceUnavailableExceptionUnmarshaller().Unmarshall(context, ref reader));
                }
            },
            {
                "ThrottlingException", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new BedrockRuntimeEventStreamException(Encoding.UTF8.GetString(payload.Payload),
                        new ThrottlingExceptionUnmarshaller().Unmarshall(context, ref reader));
                }
            },
            {
                "ValidationException", payload =>
                {
                    var context = EventStreamUtils.ConvertMessageToJsonContext(payload);
                    var reader = new StreamingUtf8JsonReader(context.Stream);
                    return new BedrockRuntimeEventStreamException(Encoding.UTF8.GetString(payload.Payload),
                        new ValidationExceptionUnmarshaller().Unmarshall(context, ref reader));
                }
            },
        };

    // Backing by a volatile bool. The flag only changes one way, so no need for a lock.
    // This is located in the subclass to be CLS compliant.
    private volatile bool _isProcessing;

    /// <summary>
    /// Whether the backround processing loop is running.
    /// </summary>
    protected override bool IsProcessing
    {
        get { return _isProcessing; }
        set { _isProcessing = value; }
    }

    /// <summary>
    /// Event that encompasses all events.
    /// </summary>
    public override event EventHandler<EventStreamEventReceivedArgs<IEventStreamEvent>> EventReceived;

    /// <summary>
    /// Event that encompasses exceptions.
    /// </summary>
    public override event EventHandler<EventStreamExceptionReceivedArgs<BedrockRuntimeEventStreamException>>
        ExceptionReceived;

    /// <summary>
    /// Event for the initial response.
    /// </summary>
    public event EventHandler<EventStreamEventReceivedArgs<InitialResponseEvent>> InitialResponseReceived;

    ///<summary>
    ///Raised when an ContentBlockDelta event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<ContentBlockDeltaEvent>> ContentBlockDeltaReceived;

    ///<summary>
    ///Raised when an ContentBlockStart event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<ContentBlockStartEvent>> ContentBlockStartReceived;

    ///<summary>
    ///Raised when an ContentBlockStop event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<ContentBlockStopEvent>> ContentBlockStopReceived;

    ///<summary>
    ///Raised when an MessageStart event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<MessageStartEvent>> MessageStartReceived;

    ///<summary>
    ///Raised when an MessageStop event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<MessageStopEvent>> MessageStopReceived;

    ///<summary>
    ///Raised when an Metadata event is received
    ///</summary>
    public event EventHandler<EventStreamEventReceivedArgs<ConverseStreamMetadataEvent>> MetadataReceived;

    /// <summary>
    /// Construct an instance
    /// </summary>
    /// <param name="stream"></param>        
    public AwsStreamOutput(Stream stream) : this(stream, null)
    {
    }

    /// <summary>
    /// Construct an instance
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="eventStreamDecoder"></param>
    public AwsStreamOutput(Stream stream, IEventStreamDecoder eventStreamDecoder) : base(stream, eventStreamDecoder)
    {
        base.EventReceived += (sender, args) => EventReceived?.Invoke(this, args);
        base.ExceptionReceived += (sender, args) => ExceptionReceived?.Invoke(this, args);

        //Mapping the generic Event to more specific Events
        Decoder.MessageReceived += (sender, args) =>
        {
            IEventStreamEvent ev;
            try
            {
                ev = ConvertMessageToEvent(args.Message);
            }
            catch (UnknownEventStreamException)
            {
                throw new UnknownEventStreamException("Received an unknown event stream type");
            }

            EventReceived?.Invoke(this, new EventStreamEventReceivedArgs<IEventStreamEvent>(ev));

            //Call RaiseEvent until it returns true or all calls complete. This way only a subset of casts are perfromed
            // and we can avoid a cascade of nested if else statements. The result is thrown away
            var _ =
                RaiseEvent(InitialResponseReceived, ev) ||
                RaiseEvent(ContentBlockDeltaReceived, ev) ||
                RaiseEvent(ContentBlockStartReceived, ev) ||
                RaiseEvent(ContentBlockStopReceived, ev) ||
                RaiseEvent(MessageStartReceived, ev) ||
                RaiseEvent(MessageStopReceived, ev) ||
                RaiseEvent(MetadataReceived, ev);
        };
    }

    private bool RaiseEvent<T>(EventHandler<EventStreamEventReceivedArgs<T>> eventHandler, IEventStreamEvent ev)
        where T : class, IEventStreamEvent
    {
        var convertedEvent = ev as T;
        if (convertedEvent != null)
        {
            eventHandler?.Invoke(this, new EventStreamEventReceivedArgs<T>(convertedEvent));
            return true;
        }

        return false;
    }
}