using System.Collections.Concurrent;
using System.Text;

namespace Thor.Abstractions;

public static class HttpClientFactory
{
    /// <summary>
    /// HttpClient池总数
    /// </summary>
    /// <returns></returns>
    private static int _poolSize;

    private static int PoolSize
    {
        get
        {
            if (_poolSize == 0)
            {
                // 获取环境变量
                var poolSize = Environment.GetEnvironmentVariable("HttpClientPoolSize");
                if (!string.IsNullOrEmpty(poolSize) && int.TryParse(poolSize, out var size))
                {
                    _poolSize = size;
                }
                else
                {
                    _poolSize = Environment.ProcessorCount;
                }

                if (_poolSize < 1)
                {
                    _poolSize = 2;
                }
            }

            return _poolSize;
        }
    }

    private static readonly ConcurrentDictionary<string, Lazy<List<HttpClient>>> HttpClientPool = new();

    public static HttpClient GetHttpClient(string key)
    {
        return HttpClientPool.GetOrAdd(key, k => new Lazy<List<HttpClient>>(() =>
        {
            var clients = new List<HttpClient>(PoolSize);

            for (var i = 0; i < PoolSize; i++)
            {
                clients.Add(new HttpClient(new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(30),
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(30),
                    EnableMultipleHttp2Connections = true,
                    ConnectTimeout = TimeSpan.FromSeconds(120000),
                    MaxAutomaticRedirections = 3,
                    AllowAutoRedirect = true,
                    Expect100ContinueTimeout = TimeSpan.FromMinutes(30),
                })
                {
                    Timeout = TimeSpan.FromMinutes(30),
                    DefaultRequestHeaders =
                    {
                        { "User-Agent", "Thor" },
                    }
                });
            }

            return clients;
        })).Value[new Random().Next(0, PoolSize)];
    }
}
