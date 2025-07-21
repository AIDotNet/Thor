namespace Thor.Abstractions.Exceptions;

public class ForbiddenException(string message) : Exception(message)
{
}