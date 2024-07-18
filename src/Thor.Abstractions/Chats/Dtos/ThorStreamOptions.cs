using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Dtos
{
    /// <summary>
    /// 流响应选项。仅当您设置 stream: true 时才设置此项。
    /// </summary>
    public class ThorStreamOptions
    {
        /// <summary>
        /// 如果设置，则会在 data: [DONE] 消息之前传输附加块。
        /// 该块上的 usage 字段显示整个请求的令牌使用统计信息， 
        /// choices 字段将始终为空数组。所有其他块也将包含一个 usage 字段，但具有空值。
        /// </summary>
        [JsonPropertyName("include_usage")]
        public bool? IncludeUsage { get; set; }
    }
}
