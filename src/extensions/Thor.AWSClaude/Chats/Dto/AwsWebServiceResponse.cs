using Amazon.Runtime;

namespace Thor.AWSClaude.Chats;

public  class AwsWebServiceResponse : AmazonWebServiceResponse
{
    private AwsStreamOutput _stream;

    /// <summary>
    /// Gets and sets the property Stream. 
    /// <para>
    /// The output stream that the model generated.
    /// </para>
    /// </summary>
    public new AwsStreamOutput Stream
    {
        get { return this._stream; }
        set { this._stream = value; }
    }

    // Check to see if Stream property is set
    internal bool IsSetStream()
    {
        return this._stream != null;
    }
}