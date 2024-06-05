using System.Collections.Concurrent;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Hunyuan.V20230901;

namespace AIDotNet.Hunyuan;

public class HunyuanFactory
{
    private static ConcurrentDictionary<string,HunyuanClient> _clients = new();
    
    public static HunyuanClient CreateClient(string secretId, string secretKey,
        string endpoint = "hunyuan.tencentcloudapi.com", string region = "ap-guangzhou")
    {
        var key = $"{secretId}|{secretKey}_{region}_{endpoint}";
        var client = _clients.GetOrAdd(key, _ =>
        {
            Credential cred = new Credential
            {
                SecretId = secretId,
                SecretKey = secretKey
            };
            // 实例化一个client选项，可选的，没有特殊需求可以跳过
            var clientProfile = new ClientProfile();
            // 实例化一个http选项，可选的，没有特殊需求可以跳过
            var httpProfile = new HttpProfile
            {
                Endpoint = endpoint
            };
        
            clientProfile.HttpProfile = httpProfile;

            // 实例化要请求产品的client对象,clientProfile是可选的
            return new HunyuanClient(cred, region, clientProfile);
        });

        return client;
    }
}