using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Dtos
{
    /// <summary>
    /// 模型调用的函数。
    /// </summary>
    public class ThorChatMessageFunction
    {
        /// <summary>
        /// 功能名,如：get_current_weather
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 调用函数所用的参数，由模型以 JSON 格式生成。请注意，该模型并不总是生成有效的 JSON，
        /// 并且可能会产生未由函数架构定义的参数。
        /// 在调用函数之前验证代码中的参数。
        /// 如："{\"location\": \"San Francisco, USA\", \"format\": \"celsius\"}"
        /// </summary>
        [JsonPropertyName("arguments")]
        public string? Arguments { get; set; }

        /// <summary>
        /// 转换参数为字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ParseArguments()
        {
            var result = string.IsNullOrWhiteSpace(Arguments) == false ? JsonSerializer.Deserialize<Dictionary<string, object>>(Arguments) : new Dictionary<string, object>();
            return result;
        }
    }
}
