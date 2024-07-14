using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Ollama.Chats.Dtos
{
    internal class OllamaChatCompletionsRequest
    {
        public string model { get; set; } = null!;

        public List<OllamaChatRequestMessage> messages { get; set; } = null!;

        public string? format { get; set; }

        public OllamaChatOptions? options { get; set; }

        public bool? stream { get; set; }

        public string? keep_alive { get; set; }
    }
}
