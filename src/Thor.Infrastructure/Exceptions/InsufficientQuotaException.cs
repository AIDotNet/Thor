namespace Thor.Service.Exceptions;

/// <summary>
/// 额度不足异常
/// </summary>
/// <param Name="message"></param>
public sealed  class InsufficientQuotaException(string message) : Exception(message);