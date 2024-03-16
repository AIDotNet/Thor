namespace TokenApi.Service.Exceptions;

/// <summary>
/// 没有模型异常
/// </summary>
/// <param name="message"></param>
public sealed class NotModelException(string? message) : Exception(message);