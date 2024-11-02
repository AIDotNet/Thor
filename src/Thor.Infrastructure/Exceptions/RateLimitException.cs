namespace Thor.Service.Exceptions;

/// <summary>
/// 限流异常
/// </summary>
/// <param name="message"></param>
public sealed class RateLimitException(string message) : Exception(message)
{
    public Dictionary<string,string> Header { get; set; }
}