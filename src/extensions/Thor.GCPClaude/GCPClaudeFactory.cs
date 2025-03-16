using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Thor.Abstractions;
using Google.Apis.Auth.OAuth2;

namespace Thor.GCPClaude
{
    public static class GCPClaudeFactory
    {
        private const string AddressTemplate = "https://{0}/v1/projects/{1}/locations/{2}/publishers/anthropic/models/{3}:{4}";

        // 令牌缓存，键为服务账号JSON的哈希，值为令牌信息
        private static readonly ConcurrentDictionary<string, TokenInfo> _tokenCache = new ConcurrentDictionary<string, TokenInfo>();

        // 用于同步令牌刷新的锁对象
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        // 令牌的默认有效期（保守估计为55分钟）
        private static readonly TimeSpan _tokenLifetime = TimeSpan.FromMinutes(55);

        public static string GetAddress(ThorPlatformOptions options, string ModelId, bool isStreaming)
        {
            if (string.IsNullOrEmpty(options.Address))
            {
                return "";
            }
            var urlParts = options!.Address!.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (urlParts.Length != 4) return "";
            var Endpoint = urlParts[0];
            var ProjectId = urlParts[1];
            var LocationId = urlParts[2];
            var Method = urlParts[3];
            if (isStreaming)
            {
                Method = "streamRawPredict";
            }
            return string.Format(AddressTemplate, Endpoint, ProjectId, LocationId, ModelId, Method);
        }

        /// <summary>
        /// 获取访问令牌（使用缓存或刷新）
        /// </summary>
        public static string GetToken(ThorPlatformOptions options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return "";
            }

            // 同步方法中调用异步方法
            return GetTokenAsync(options).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步获取访问令牌（使用缓存或刷新）
        /// </summary>
        public static async Task<string> GetTokenAsync(ThorPlatformOptions options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return "";
            }

            string serviceAccountJson = options.ApiKey;

            // 生成缓存键
            string cacheKey = GenerateCacheKey(serviceAccountJson);

            // 获取该键对应的同步锁
            var lockObj = _locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

            // 检查是否存在有效的缓存令牌
            if (_tokenCache.TryGetValue(cacheKey, out var tokenInfo) && !IsTokenExpired(tokenInfo))
            {
                return tokenInfo.Token;
            }

            // 需要刷新令牌，获取锁
            await lockObj.WaitAsync();
            try
            {
                // 再次检查，防止在等待锁的过程中已被其他线程刷新
                if (_tokenCache.TryGetValue(cacheKey, out tokenInfo) && !IsTokenExpired(tokenInfo))
                {
                    return tokenInfo.Token;
                }

                // 刷新令牌
                return await RefreshTokenAsync(serviceAccountJson, cacheKey);
            }
            finally
            {
                lockObj.Release();
            }
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        private static async Task<string> RefreshTokenAsync(string serviceAccountJson, string cacheKey)
        {
            try
            {
                // 从JSON字符串加载Google凭据
                GoogleCredential credential;
                try
                {
                    credential = GoogleCredential.FromJson(serviceAccountJson);
                }
                catch (Exception)
                {
                    // 如果不是有效的JSON，尝试作为文件路径处理
                    if (File.Exists(serviceAccountJson))
                    {
                        using var stream = File.OpenRead(serviceAccountJson);
                        credential = GoogleCredential.FromStream(stream);
                    }
                    else
                    {
                        throw new Exception("无效的GCP服务账号密钥或文件路径");
                    }
                }

                // 设置访问范围
                credential = credential.CreateScoped(new[] { "https://www.googleapis.com/auth/cloud-platform" });

                // 获取访问令牌
                string token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                // 计算过期时间
                DateTime expiry = DateTime.UtcNow.Add(_tokenLifetime);

                // 创建新的令牌信息
                var newTokenInfo = new TokenInfo { Token = token, ExpiryTime = expiry };

                // 更新缓存
                _tokenCache[cacheKey] = newTokenInfo;

                return token;
            }
            catch (Exception ex)
            {
                // 生产环境中应该使用适当的日志记录
                Console.WriteLine($"获取访问令牌失败: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// 检查令牌是否已过期（添加5分钟缓冲区）
        /// </summary>
        private static bool IsTokenExpired(TokenInfo tokenInfo)
        {
            // 提前5分钟刷新令牌，避免边界情况
            return DateTime.UtcNow.AddMinutes(5) >= tokenInfo.ExpiryTime;
        }

        /// <summary>
        /// 为服务账号生成唯一缓存键
        /// </summary>
        private static string GenerateCacheKey(string serviceAccountJson)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(serviceAccountJson);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // 转换为十六进制字符串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// 强制刷新特定服务账号的令牌
        /// </summary>
        public static async Task<string> ForceRefreshTokenAsync(ThorPlatformOptions options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return "";
            }

            string serviceAccountJson = options.ApiKey;
            string cacheKey = GenerateCacheKey(serviceAccountJson);

            var lockObj = _locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

            await lockObj.WaitAsync();
            try
            {
                return await RefreshTokenAsync(serviceAccountJson, cacheKey);
            }
            finally
            {
                lockObj.Release();
            }
        }

        /// <summary>
        /// 清除所有缓存的令牌（用于测试或重置）
        /// </summary>
        public static void ClearTokenCache()
        {
            _tokenCache.Clear();
        }

        /// <summary>
        /// 存储令牌信息和过期时间
        /// </summary>
        private class TokenInfo
        {
            public string Token { get; set; }
            public DateTime ExpiryTime { get; set; }
        }
    }
}
