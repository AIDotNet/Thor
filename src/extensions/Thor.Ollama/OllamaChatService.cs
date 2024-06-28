using System.Diagnostics;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;
using OpenAI.ObjectModels.RequestModels;

namespace Thor.Ollama
{
    public class OllamaChatService(IHttpClientFactory httpClientFactory) : IApiChatCompletionService
    {
        private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(OllamaOptions.ServiceName));

        public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var client = HttpClient;

            var response = await client.PostJsonAsync((options?.Address?.TrimEnd('/') ?? "") + "/api/chat", new OllamaChatRequest()
            {
                stream = false,
                model = chatCompletionCreate.Model ?? "",
                messages = chatCompletionCreate.Messages.Select(x => new OllamaChatRequestMessage()
                {
                    role = x.Role,
                    content = x.Content ?? ""
                }).ToList(),
                options = new OllamaChatOptions()
                {
                    stop = chatCompletionCreate.StopCalculated?.FirstOrDefault(),
                    top_p = chatCompletionCreate.TopP,
                    temperature = chatCompletionCreate.Temperature,
                }
            }, options?.Key ?? "");

            OllamaChatResponse? result;
            var data = await response.Content.ReadAsStringAsync(cancellationToken);
            try
            {
                result = JsonSerializer.Deserialize<OllamaChatResponse>(data);
                if (result == null)
                {
                    throw new Exception("ollama chat result null");
                }
            }
            catch (Exception)
            {
                Debug.WriteLine($"ollama chat result error: {data}");
                throw;
            }

            return new ChatCompletionCreateResponse()
            {
                Model = result.model,
                Choices = result.message == null ? [] :
                [
                    new ChatChoiceResponse()
                    {
                        Delta = new ChatMessage()
                        {
                            Role = result.message.role,
                            Content = result.message.content
                        },
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Usage = new UsageResponse()
                {
                    PromptTokens = result.prompt_eval_count ?? 0,
                    CompletionTokens = result.eval_count ?? 0,
                    TotalTokens = (result.prompt_eval_count ?? 0) + (result.eval_count ?? 0)
                }
            };
        }

        public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var client = HttpClient;

            var response = await client.HttpRequestRaw((options?.Address?.TrimEnd('/') ?? "") + "/api/chat", new OllamaChatRequest()
            {
                stream = true,
                model = chatCompletionCreate.Model ?? "",
                messages = chatCompletionCreate.Messages.Select(x => new OllamaChatRequestMessage()
                {
                    role = x.Role,
                    content = x.Content ?? ""
                }).ToList(),
                options = new OllamaChatOptions()
                {
                    stop = chatCompletionCreate.StopCalculated?.FirstOrDefault(),
                    top_p = chatCompletionCreate.TopP,
                    temperature = chatCompletionCreate.Temperature,
                }
            }, options?.Key ?? "");

            using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
            string? line = string.Empty;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                OllamaChatResponse? result;
                try
                {
                    result = JsonSerializer.Deserialize<OllamaChatResponse>(line);
                }
                catch (Exception)
                {
                    Debug.WriteLine($"ollama chat result error: {line}");
                    throw;
                }

                if (result == null)
                    continue;

                if (result.done)
                {
                    yield return new ChatCompletionCreateResponse()
                    {
                        Model = result.model,
                        Choices = [],
                        Usage = new UsageResponse()
                        {
                            PromptTokens = result.prompt_eval_count ?? 0,
                            CompletionTokens = result.eval_count ?? 0,
                            TotalTokens = (result.prompt_eval_count ?? 0) + (result.eval_count ?? 0)
                        }
                    };
                    break;
                }
                else
                {
                    yield return new ChatCompletionCreateResponse()
                    {
                        Model = result.model,
                        Choices = result.message == null ? [] :
                        [
                            new ChatChoiceResponse()
                            {
                                Delta = new ChatMessage()
                                {
                                    Role = result.message.role,
                                    Content = result.message.content
                                },
                                FinishReason = "stop",
                                Index = 0,
                            }
                        ],
                        Usage = new UsageResponse()
                        {
                            PromptTokens = result.prompt_eval_count ?? 0,
                            CompletionTokens = result.eval_count ?? 0,
                            TotalTokens = (result.prompt_eval_count ?? 0) + (result.eval_count ?? 0)
                        }
                    };
                }
            }
        }
    }

    public class OllamaChatRequest
    {
        public string model { get; set; } = null!;

        public List<OllamaChatRequestMessage> messages { get; set; } = null!;

        public string? format { get; set; }

        public OllamaChatOptions? options { get; set; }

        public bool? stream { get; set; }

        public string? keep_alive { get; set; }
    }

    public class OllamaChatOptions
    {
        public int? mirostat { get; set; }
        public float? mirostat_eta { get; set; }
        public float? mirostat_tau { get; set; }
        public int? num_ctx { get; set; }
        public int? repeat_last_n { get; set; }
        public float? repeat_penalty { get; set; }
        public float? temperature { get; set; }
        public int? seed { get; set; }
        public string? stop { get; set; }
        public float? tfs_z { get; set; }
        public int? num_predict { get; set; }
        public int? top_k { get; set; }
        public float? top_p { get; set; }
    }

    public class OllamaChatRequestMessage
    {
        public string role { get; set; } = null!;

        public string content { get; set; } = null!;

        public List<string>? images { get; set; }
    }

    public class OllamaChatResponse
    {
        public string model { get; set; } = null!;
        public DateTime created_at { get; set; }
        public OllamaChatResponseMessage? message { get; set; }
        public bool done { get; set; }
        public string done_reason { get; set; }
        public long? total_duration { get; set; }
        public int? load_duration { get; set; }
        public int? prompt_eval_count { get; set; }
        public int? prompt_eval_duration { get; set; }
        public int? eval_count { get; set; }
        public long? eval_duration { get; set; }
    }

    public class OllamaChatResponseMessage
    {
        public string role { get; set; } = null!;
        public string content { get; set; } = null!;
        public List<string>? Images { get; set; }
    }
}
