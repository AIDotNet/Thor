using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Ollama.Chats.Dtos;

internal class OllamaChatResponse
{
    public string model { get; set; } = null!;
    public DateTime created_at { get; set; }
    public OllamaChatResponseMessage? message { get; set; }
    public bool done { get; set; }
    public string done_reason { get; set; }
    public long? total_duration { get; set; }
    public long? load_duration { get; set; }
    public int? prompt_eval_count { get; set; }
    public int? prompt_eval_duration { get; set; }
    public int? eval_count { get; set; }
    public long? eval_duration { get; set; }
}
