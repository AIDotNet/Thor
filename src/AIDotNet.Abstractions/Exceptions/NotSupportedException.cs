namespace TokenApi.Service.Exceptions;

/// <summary>
/// 未支持的功能异常
/// </summary>
/// <param name="message"></param>
public sealed class ModelNotSupportedException(string? message) : Exception(message);