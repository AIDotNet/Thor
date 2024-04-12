using Sdcb.SparkDesk;

namespace AIDotNet.SparkDesk;

public static class SparkDeskHelper
{
    /// <summary>
    /// 解析API Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    
    public static SparkDeskClient GetSparkDeskClient(string key)
    {
        SparkDeskClient client;
        // appId|appKey|appSecret
        var parts = key.Split('|');
        if (parts.Length == 3)
        {
            client = new SparkDeskClient(parts[0], parts[1], parts[2]);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }
        
        return client;
    }
}