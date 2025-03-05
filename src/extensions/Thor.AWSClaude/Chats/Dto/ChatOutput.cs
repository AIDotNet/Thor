using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Thor.AWSClaude.Chats.Dto;

public class ChatOutput
{
    private ChatOutputMessage _message;

    /// <summary>
    /// Gets and sets the property Message. 
    /// <para>
    /// The message that the model generates.
    /// </para>
    /// </summary>
    public ChatOutputMessage Message
    {
        get { return this._message; }
        set { this._message = value; }
    }

    // Check to see if Message property is set
    internal bool IsSetMessage()
    {
        return this._message != null;
    }
}

public class ChatOutputMessage
{
    private List<ReasoningContentBlockDelta> _content = AWSConfigs.InitializeCollections ? new List<ReasoningContentBlockDelta>() : (List<ReasoningContentBlockDelta>) null;
    private ConversationRole _role;

    /// <summary>
    /// Gets and sets the property Content.
    /// <para>
    /// The message content. Note the following restrictions:
    /// </para>
    ///  <ul> <li>
    /// <para>
    /// You can include up to 20 images. Each image's size, height, and width must be no more
    /// than 3.75 MB, 8000 px, and 8000 px, respectively.
    /// </para>
    ///  </li> <li>
    /// <para>
    /// You can include up to five documents. Each document's size must be no more than 4.5
    /// MB.
    /// </para>
    ///  </li> <li>
    /// <para>
    /// If you include a <c>ContentBlock</c> with a <c>document</c> field in the array, you
    /// must also include a <c>ContentBlock</c> with a <c>text</c> field.
    /// </para>
    ///  </li> <li>
    /// <para>
    /// You can only include images and documents if the <c>role</c> is <c>user</c>.
    /// </para>
    ///  </li> </ul>
    /// </summary>
    [AWSProperty(Required = true)]
    public List<ReasoningContentBlockDelta> Content
    {
        get => this._content;
        set => this._content = value;
    }

    internal bool IsSetContent()
    {
        if (this._content == null)
            return false;
        return this._content.Count > 0 || !AWSConfigs.InitializeCollections;
    }

    /// <summary>
    /// Gets and sets the property Role.
    /// <para>
    /// The role that the message plays in the message.
    /// </para>
    /// </summary>
    [AWSProperty(Required = true)]
    public ConversationRole Role
    {
        get => this._role;
        set => this._role = value;
    }

    internal bool IsSetRole() => (ConstantClass) this._role != (ConstantClass) null;
}