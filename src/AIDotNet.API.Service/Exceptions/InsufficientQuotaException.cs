namespace AIDotNet.API.Service.Exceptions;

/// <summary>
/// 额度不足异常
/// </summary>
/// <param name="message"></param>
public class InsufficientQuotaException(string message) : Exception(message);