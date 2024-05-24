using AIDotNet.Abstractions.Exceptions;
using AIDotNet.SparkDesk.API;

namespace AIDotNet.SparkDesk;

public static class SparkDeskHelper
{
    /// <summary>
    /// 解析API Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static XFSparkDeskChatAPI GetSparkDeskChatClient(string key, string model, string? url = null)
    {
        XFSparkDeskChatAPI client;
        // appId|appKey|appSecret
        var parts = key.Split('|');
        if (parts.Length == 3)
        {
            XFSparkDeskModelType modelVersion;
            string hostURL = "";
            if (model is "SparkDesk-v3.5" or "generalv3.5")
            {
                modelVersion = XFSparkDeskModelType.V3_5;
                hostURL = url ?? XFSparkDeskHostURL.Chat_V3_5;
            }
            else if (model is "SparkDesk-v3.1" or "generalv3")
            {
                modelVersion = XFSparkDeskModelType.V3_0;
                hostURL = url ?? XFSparkDeskHostURL.Chat_V3_0;
            }
            else if (model is "SparkDesk-v1.5" or "general")
            {
                modelVersion = XFSparkDeskModelType.V1_5;
                hostURL = url ?? XFSparkDeskHostURL.Chat_V1_5;
            }
            else if (model is "SparkDesk-v2.1" or "generalv2")
            {
                modelVersion = XFSparkDeskModelType.V2_0;
                hostURL = url ?? XFSparkDeskHostURL.Chat_V2_0;
            }
            else
            {
                throw new NotModelException(model);
            }

            client = new XFSparkDeskChatAPI(new XFSparkDeskChatAPIConfig()
            {
                AppId = parts[0],
                ApiKey = parts[1],
                ApiSecret = parts[2],
                ModeType = modelVersion,
                HostURL = hostURL
            });
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        return client;
    }

    public static XFSparkDeskImageGenerationAPI GetSparkDeskImageGenerationClient(string key, HttpClient httpClient,
        string? url = null)
    {
        XFSparkDeskImageGenerationAPI client;
        // appId|appKey|appSecret
        var parts = key.Split('|');
        if (parts.Length == 3)
        {
            client = new XFSparkDeskImageGenerationAPI(new XFSparkDeskAPIConfig()
            {
                AppId = parts[0],
                ApiKey = parts[1],
                ApiSecret = parts[2],
                HostURL = url ?? XFSparkDeskHostURL.ImageGeneration_V2_1
            }, httpClient);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        return client;
    }

    public static XFSparkDeskEmbeddingAPI GetSparkDeskEmbeddingClient(string key, HttpClient httpClient,
        string? url = null)
    {
        XFSparkDeskEmbeddingAPI client;
        // appId|appKey|appSecret
        var parts = key.Split('|');
        if (parts.Length == 3)
        {
            client = new XFSparkDeskEmbeddingAPI(new XFSparkDeskAPIConfig()
            {
                AppId = parts[0],
                ApiKey = parts[1],
                ApiSecret = parts[2],
                HostURL = url ?? XFSparkDeskHostURL.Embedding
            }, httpClient);
        }
        else
        {
            throw new ArgumentException("Invalid API Key format, expected appId|appKey|appSecret");
        }

        return client;
    }
}