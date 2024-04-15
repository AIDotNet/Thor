namespace AIDotNet.Abstractions.Exceptions;

/// <summary>
/// 没有模型异常
/// </summary>
/// <param Name="message"></param>
public sealed class NotModelException(string? message) : Exception(message);