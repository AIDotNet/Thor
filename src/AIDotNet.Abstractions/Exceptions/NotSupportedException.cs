namespace TokenApi.Service.Exceptions;

/// <summary>
/// 未支持的功能异常
/// </summary>
/// <param Name="message"></param>
public sealed class ModelNotSupportedException(string? message) : Exception(message);