namespace AIDotNet.API.Service.Options;

public sealed class OpenAIOptions
{
    public const string Name = "OpenAI";
    
    /// <summary>
    /// 端点
    /// </summary>
    public static string Endpoint { get; set; }
    
    /// <summary>
    /// 密钥
    /// </summary>
    public static string ApiKey { get; set; }
}