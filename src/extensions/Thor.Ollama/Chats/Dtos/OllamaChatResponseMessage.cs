using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Ollama.Chats.Dtos
{
    internal class OllamaChatResponseMessage
    {
        public string role { get; set; } = null!;
        public string content { get; set; } = null!;
        public List<string>? Images { get; set; }
    }
}
