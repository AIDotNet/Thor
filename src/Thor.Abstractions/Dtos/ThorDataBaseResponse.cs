using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Thor.Abstractions.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record ThorDataBaseResponse<T> : ThorBaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("data")] 
        public T? Data { get; set; }
    }
}
