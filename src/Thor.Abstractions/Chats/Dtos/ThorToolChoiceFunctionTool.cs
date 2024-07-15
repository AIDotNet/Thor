using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Thor.Abstractions.Chats.Dtos
{
    public class ThorToolChoiceFunctionTool
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
