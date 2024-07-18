using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.SparkDesk.Helpers
{
    public static class SparkDeskApiKeyHelper
    {
        /// <summary>
        /// 解析传入的 thorApiKey
        /// </summary>
        /// <param name="thorApiKey">格式：APPID|APIKey|APISecret</param>
        /// <returns></returns>
        /// <example>
        /// (string appId, string apiKey, string apiSecret) = ParseThorApiKey("");
        /// </example>
        public static (string appId, string apiKey, string apiSecret) ParseThorApiKey(string thorApiKey)
        {
            var arr = thorApiKey.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 3)
            {
                throw new ArgumentException("thorApiKey 格式不是APPID|APIKey|APISecret");
            }

            return (arr[0], arr[1], arr[2]);
        }
    }
}
