namespace Thor.Service.Options;

public class TrackerOptions
{
    public const string Tracker = "Tracker";

    public static bool Enable { get; set; }

    /// <summary>
    /// 请求时使用的密钥
    /// </summary>
    public static string ApiKey { get; set; }

    /// <summary>
    /// 请求时使用的模型
    /// </summary>
    /// <returns></returns>
    public static string Model { get; set; } 
    
    /// <summary>
    /// 请求时使用的端点
    /// http://127.0.0.1:8080
    /// </summary>
    public static string Endpoint { get; set; }
}