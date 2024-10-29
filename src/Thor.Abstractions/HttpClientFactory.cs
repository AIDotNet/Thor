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
                    _poolSize = Environment.ProcessorCount * 2;
                }

                if (_poolSize < 1)
                {
                    _poolSize = 5;
                }
            }

            return _poolSize;
        }
    }

    private static readonly List<HttpClient> HttpClients = new();

    static HttpClientFactory()
    {
        for (var i = 0; i < PoolSize; i++)
        {
            HttpClients.Add(new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
                EnableMultipleHttp2Connections = true,
                ConnectTimeout = TimeSpan.FromMinutes(10),
                KeepAlivePingTimeout = TimeSpan.FromMinutes(10),
                ResponseDrainTimeout = TimeSpan.FromMinutes(10),
            })
            {
                Timeout = TimeSpan.FromMinutes(10),
                DefaultRequestHeaders =
                {
                    { "User-Agent", "Thor" },
                }
            });
        }
    }

    public static HttpClient HttpClient => HttpClients[new Random().Next(0, PoolSize)];
}