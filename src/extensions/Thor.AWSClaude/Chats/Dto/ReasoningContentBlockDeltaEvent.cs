using System.Text.Json.Serialization;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.EventStreams.Internal;
using Amazon.Runtime.Internal;
using Thor.AWSClaude.Chats.Dto;

namespace Thor.AWSClaude.Chats;

public class ReasoningContentBlockDeltaEvent : IEventStreamEvent
{
    private int? _contentBlockIndex;
    private ReasoningContentBlockDelta _delta;

    /// <summary>
    /// Gets and sets the property ContentBlockIndex. 
    /// <para>
    /// The block index for a content block delta event. 
    /// </para>
    /// </summary>
    [AWSProperty(Required = true, Min = 0)]
    public int? ContentBlockIndex
    {
        get { return this._contentBlockIndex; }
        set { this._contentBlockIndex = value; }
    }

    // Check to see if ContentBlockIndex property is set
    internal bool IsSetContentBlockIndex()
    {
        return this._contentBlockIndex.HasValue;
    }

    /// <summary>
    /// Gets and sets the property Delta. 
    /// <para>
    /// The delta for a content block delta event.
    /// </para>
    /// </summary>
    [AWSProperty(Required = true)]
    [JsonPropertyName("delta")]
    public ReasoningContentBlockDelta Delta
    {
        get { return this._delta; }
        set { this._delta = value; }
    }

    // Check to see if Delta property is set
    internal bool IsSetDelta()
    {
        return this._delta != null;
    }
}

public class ReasoningContentBlockDelta
{
    private string _text;
    private AwsStreamResponseContentToolUse? _toolUse;

    /// <summary>
    /// Gets and sets the property Text. 
    /// <para>
    /// The content text.
    /// </para>
    /// </summary>
    public string Text
    {
        get { return this._text; }
        set { this._text = value; }
    }

    // Check to see if Text property is set
    internal bool IsSetText()
    {
        return !string.IsNullOrEmpty(this._text);
    }

    /// <summary>
    /// Gets and sets the property ToolUse. 
    /// <para>
    /// Information about a tool that the model is requesting to use.
    /// </para>
    /// </summary>
    public AwsStreamResponseContentToolUse? ToolUse
    {
        get { return this._toolUse; }
        set { this._toolUse = value; }
    }

    private ReasoningContent? reasoningContent;


    [JsonPropertyName("reasoningContent")]
    public ReasoningContent? ReasoningContent
    {
        get => reasoningContent;
        set => reasoningContent = value;
    }
}

public class ReasoningContent
{
    public string Text { get; set; }
}