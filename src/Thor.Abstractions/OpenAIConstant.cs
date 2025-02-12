namespace Thor.Abstractions;

/// <summary>
/// OpenAI常量
/// </summary>
public static class OpenAIConstant
{
    /// <summary>
    /// 字符串utf-8编码
    /// </summary>
    /// <returns></returns>
    public const string Done = "[DONE]";
    
    /// <summary>
    /// Data: 协议头
    /// </summary>
    public const string Data = "data:";
    
    /// <summary>
    /// think: 协议头
    /// </summary>
    public const string ThinkStart = "<think>";
    
    /// <summary>
    /// think: 协议尾
    /// </summary>
    public const string ThinkEnd = "</think>";
}