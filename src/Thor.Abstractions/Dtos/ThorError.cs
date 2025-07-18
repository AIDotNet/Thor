using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thor.Abstractions.Dtos
{
    public class ThorError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonPropertyName("code")]
        public object? Code { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [JsonPropertyName("param")]
        public string? Param { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonIgnore]
        public string? Message { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonIgnore]
        public List<string?> Messages { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonPropertyName("message")]
        public object MessageObject
        {
            set
            {
                switch (value)
                {
                    case string s:
                        Message = s;
                        Messages = new() { s };
                        break;
                    case List<object> list when list.All(i => i is JsonElement):
                        Messages = list.Cast<JsonElement>().Select(e => e.GetString()).ToList();
                        Message = string.Join(Environment.NewLine, Messages);
                        break;
                }
            }
            
            get
            {
                if (Messages?.Count > 1)
                {
                    return Messages;
                }

                return Message;
            }
        }
    }
}
