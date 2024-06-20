namespace Thor.Service.Exceptions;

/// <summary>
/// 通道异常
/// </summary>
/// <param name="message"></param>
public sealed class ChannelException(string message) : Exception(message)
{
}