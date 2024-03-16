namespace AIDotNet.AliyunFC;

public class AliyunFCOptions
{
    public const string ServiceName = "AliyunFC";

    /// <summary>
    /// Fc app key
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Fc endpoint
    /// </summary>
    public string ApiUrl { get; set; }
    
    public HttpClient? HttpClient { get; set; }
    
    public IServiceProvider ServiceProvider { get; set; }
}