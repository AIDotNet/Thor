namespace Thor.Abstractions.Exceptions;

public class ThorRateLimitException : Exception
{
    public ThorRateLimitException()
    {
    }

    public ThorRateLimitException(string message) : base(message)
    {
    }
}