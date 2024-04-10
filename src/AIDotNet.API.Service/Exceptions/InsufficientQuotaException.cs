namespace AIDotNet.API.Service.Exceptions;

/// <summary>
/// 额度不足异常
/// </summary>
/// <param Name="message"></param>
public class InsufficientQuotaException(string message) : Exception(message);