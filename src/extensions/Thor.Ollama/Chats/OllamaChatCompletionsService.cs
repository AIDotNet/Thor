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
    public class OllamaChatCompletionsService
        : IThorChatCompletionsService
    {
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
            var url = (options?.Address?.TrimEnd('/') ?? "") + "/api/chat";

            var tools = new List<Tool>();

            if (request.Tools != null)
            {
                foreach (var tool in request.Tools)
                {
                    var properties = new Dictionary<string, Properties>();
                    foreach (var definition in tool?.Function?.Parameters?.Properties)
                    {
                        properties.Add(definition.Key, new Properties()
                        {
                            Description = definition.Value.Description ?? string.Empty,
                            Type = definition.Value.Type,
                            Enum = definition.Value.Enum?.ToArray(),
                        });
                    }

                    tools.Add(new Tool
                    {
                        Function = new Function
                        {
                            Description = tool?.Function.Description,
                            Name = tool?.Function.Name,
                            Parameters = new Parameters
                            {
                                Properties = properties,
                                Required = tool?.Function?.Parameters?.Required?.ToArray(),
                                Type = tool?.Function?.Parameters?.Type,
                            }
                        }
                    });
                }
            }

            var response = await HttpClientFactory.HttpClient.PostJsonAsync(url, new OllamaChatCompletionsRequest()
            {
                stream = false,
                model = request.Model ?? "", 
                Tools = tools,
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

            var toolsResult = new List<ThorToolCall>();
            if (result.message?.ToolCalls!= null && result.message.ToolCalls.Count() >0)
            {
                foreach (var content in result.message.ToolCalls)
                {
                    toolsResult.Add(new ThorToolCall()
                    {    
                        Function = new ThorChatMessageFunction()
                        {
                            Arguments = JsonSerializer.Serialize(content.Function?.Arguments),
                            Name = content.Function?.Name
                        }
                    });
                }
            }

            var message = ThorChatMessage.CreateAssistantMessage(result.message?.content ?? string.Empty, toolCalls: toolsResult);
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
            var tools = new List<Tool>();

            if (request.Tools != null)
            {
                foreach (var tool in request.Tools)
                {
                    var properties = new Dictionary<string, Properties>();
                    foreach (var definition in tool?.Function?.Parameters?.Properties)
                    {
                        properties.Add(definition.Key, new Properties()
                        {
                            Description = definition.Value.Description ?? string.Empty,
                            Type = definition.Value.Type,
                            Enum = definition.Value.Enum?.ToArray(),
                        });
                    }

                    tools.Add(new Tool
                    {
                        Function = new Function
                        {
                            Description = tool?.Function.Description,
                            Name = tool?.Function.Name,
                            Parameters = new Parameters
                            {
                                Properties = properties,
                                Required = tool?.Function?.Parameters?.Required?.ToArray(),
                                Type = tool?.Function?.Parameters?.Type,
                            }
                        }
                    });
                }
            }
            var response = await HttpClientFactory.HttpClient.HttpRequestRaw((options?.Address?.TrimEnd('/') ?? "") + "/api/chat", new OllamaChatCompletionsRequest()
            {
                stream = true,
                model = request.Model ?? "",
                Tools = tools,
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
                    var toolsResult = new List<ThorToolCall>();
                    if (result.message?.ToolCalls != null && result.message.ToolCalls.Count() > 0)
                    {
                        foreach (var content in result.message.ToolCalls)
                        {
                            toolsResult.Add(new ThorToolCall()
                            {
                                Function = new ThorChatMessageFunction()
                                {
                                    Arguments = JsonSerializer.Serialize(content.Function?.Arguments),
                                    Name = content.Function?.Name
                                }
                            });
                        }
                    }

                    var message = ThorChatMessage.CreateAssistantMessage(result.message?.content ?? string.Empty, toolCalls: toolsResult);

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
