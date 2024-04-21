using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.Ollama
{
    public sealed class OllamaOptions
    {
        public const string ServiceName = "Ollama";

        public IServiceProvider ServiceProvider { get; set; }
    }
}
