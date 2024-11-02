namespace Thor.Service.Exceptions;

/// <summary>
/// OpenAI异常内容。
/// </summary>
/// <param name="message"></param>
public sealed class OpenAIErrorException(string message, string code) : Exception(message)
{
    public string Code { get; } = code;
    
    public override string Message { get; } = message;
}