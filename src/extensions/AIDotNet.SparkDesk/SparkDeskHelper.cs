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

    public static XFSparkDeskChatAPI GetSparkDeskClient(string key, string? model)
    {
        XFSparkDeskChatAPI client;
        // appId|appKey|appSecret
        var parts = key.Split('|');
        if (parts.Length == 3)
        {
            XFSparkDeskModelType modelVersion;
            string hostURL = "";
            if (model == "SparkDesk-v3.5")
            {
                modelVersion = XFSparkDeskModelType.V3_5;
                hostURL = XFSparkDeskHostURL.Chat_V3_5;
            }
            else if (model == "SparkDesk-v3.1")
            {
                modelVersion = XFSparkDeskModelType.V3_0;
                hostURL = XFSparkDeskHostURL.Chat_V3_0;
            }
            else if (model == "SparkDesk-v1.5")
            {
                modelVersion = XFSparkDeskModelType.V1_5;
                hostURL = XFSparkDeskHostURL.Chat_V1_5;
            }
            else if (model == "SparkDesk-v2.1")
            {
                modelVersion = XFSparkDeskModelType.V2_0;
                hostURL = XFSparkDeskHostURL.Chat_V2_0;
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
}