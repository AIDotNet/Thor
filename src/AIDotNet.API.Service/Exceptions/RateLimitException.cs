namespace AIDotNet.API.Service.Exceptions;

public class RateLimitException(string message) : Exception(message)
{
    public Dictionary<string,string> Header { get; set; }
}