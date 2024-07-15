using System.Diagnostics;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Extensions;
using Thor.Ollama.Chats.Dtos;

namespace Thor.Ollama.Chats
{
    /// <summary>
    /// Ollama 对话补全服务实现
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public class OllamaChatCompletionsService(IHttpClientFactory httpClientFactory)
        : IThorChatCompletionsService
    {
        /// <summary>
        /// http 客户端
        /// </summary>
        private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(OllamaPlatformOptions.PlatformCode));

        /// <summary>
        /// 非流式对话补全
        /// </summary>
        /// <param name="request">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(
            ThorChatCompletionsRequest request, 
            ThorPlatformOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            var client = HttpClient;

            var response = await client.PostJsonAsync((options?.Address?.TrimEnd('/') ?? "") + "/api/chat", new OllamaChatCompletionsRequest()
            {
                stream = false,
                model = request.Model ?? "",
                messages = request.Messages.Select(x => new OllamaChatRequestMessage()
                {
                    role = x.Role,
                    content = x.Content ?? ""
                }).ToList(),
                options = new OllamaChatOptions()
                {
                    stop = request.StopCalculated?.FirstOrDefault(),
                    top_p = request.TopP,
                    temperature = request.Temperature,
                }
            }, options?.ApiKey ?? "");

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
            catch (Exception ex)
            {
                Debug.WriteLine($"ollama chat result error: {data}");
                throw;
            }

            var message = ThorChatMessage.CreateAssistantMessage(result.message.content);
            return new ThorChatCompletionsResponse()
            {
                Model = result.model,
                Choices = result.message == null ? [] :
                [
                    new ThorChatChoiceResponse()
                    {
                        Delta =message,
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Usage = new ThorUsageResponse()
                {
                    PromptTokens = result.prompt_eval_count ?? 0,
                    CompletionTokens = result.eval_count ?? 0,
                    TotalTokens = (result.prompt_eval_count ?? 0) + (result.eval_count ?? 0)
                }
            };
        }

        /// <summary>
        /// 流式对话补全
        /// </summary>
        /// <param name="request">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
            ThorChatCompletionsRequest request, 
            ThorPlatformOptions? options = null, 
            CancellationToken cancellationToken = default)
        {
            var client = HttpClient;

            var response = await client.HttpRequestRaw((options?.Address?.TrimEnd('/') ?? "") + "/api/chat", new OllamaChatCompletionsRequest()
            {
                stream = true,
                model = request.Model ?? "",
                messages = request.Messages.Select(x => new OllamaChatRequestMessage()
                {
                    role = x.Role,
                    content = x.Content ?? ""
                }).ToList(),
                options = new OllamaChatOptions()
                {
                    stop = request.StopCalculated?.FirstOrDefault(),
                    top_p = request.TopP,
                    temperature = request.Temperature,
                }
            }, options?.ApiKey ?? "");

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
                    yield return new ThorChatCompletionsResponse()
                    {
                        Model = result.model,
                        Choices = [],
                        Usage = new ThorUsageResponse()
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
                    var message = ThorChatMessage.CreateAssistantMessage(result.message.content);

                    yield return new ThorChatCompletionsResponse()
                    {
                        Model = result.model,
                        Choices = result.message == null ? [] :
                        [
                            new ThorChatChoiceResponse()
                            {
                                Delta =message,
                                FinishReason = "stop",
                                Index = 0,
                            }
                        ],
                        Usage = new ThorUsageResponse()
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
}
