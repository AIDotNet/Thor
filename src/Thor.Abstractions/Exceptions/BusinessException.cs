namespace Thor.Abstractions.Exceptions;

public class BusinessException : Exception
{
    private string Code;

    public BusinessException(string message, string code) : base(message)
    {
        Code = code;
    }
}