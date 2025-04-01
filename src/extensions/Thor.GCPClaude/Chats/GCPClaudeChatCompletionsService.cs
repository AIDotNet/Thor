using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.GCPClaude.Chats.Dto;

namespace Thor.GCPClaude.Chats
{
    public sealed class GCPClaudeChatCompletionsService(ILogger<GCPClaudeChatCompletionsService> logger) : IThorChatCompletionsService
    {
        /// <summary>
        /// 非流式对话补全
        /// </summary>
        /// <param name="input">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
            ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using var openai = Activity.Current?.Source.StartActivity("GCPClaude 对话补全");

            bool isThink = input.Model.EndsWith("-thinking");
            input.Model = input.Model.Replace("-thinking", string.Empty);

            // 获取地址和token
            var (url, token) = GetUrlAndToken(options, input.Model, false);

            // 构建请求对象
            var request = BuildClaudeRequest(input, false, isThink);

            // 发送请求
            var responseMessage = await HttpClientFactory.GetHttpClient(url)
                                  .PostJsonAsync(url, request, token, "Authorization");

            // 处理响应
            await HandleResponseErrors(responseMessage, url, cancellationToken);

            // 解析响应
            var value = await responseMessage.Content
                                .ReadFromJsonAsync<ClaudeChatCompletionDto>(ThorJsonSerializer.DefaultOptions,
                                cancellationToken: cancellationToken)
                                .ConfigureAwait(false);
            if (value == null)
            {
                throw new Exception("Failed to deserialize Claude API response.");
            }

            var thor = new ThorChatCompletionsResponse()
            {
                Choices = CreateResponse(value),
                Model = value.model,
                Id = value.id,
                Usage = new ThorUsageResponse()
                {
                    CompletionTokens = value.usage.output_tokens,
                    PromptTokens = value.usage.input_tokens,
                    TotalTokens = value.usage.output_tokens + value.usage.input_tokens
                }
            };

            if (value.usage.cache_read_input_tokens != null)
            {
                thor.Usage.PromptTokensDetails ??= new ThorUsageResponsePromptTokensDetails()
                {
                    CachedTokens = value.usage.cache_read_input_tokens.Value,
                };
            }

            return thor;
        }

        /// <summary>
        /// 流式对话补全
        /// </summary>
        /// <param name="input">对话补全请求参数对象</param>
        /// <param name="options">平台参数对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest input,
            ThorPlatformOptions? options = null,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var openai = Activity.Current?.Source.StartActivity("GCPClaude 流式对话补全");

            bool isThink = input.Model.EndsWith("-thinking");
            input.Model = input.Model.Replace("-thinking", string.Empty);
            // 获取地址和token
            var (url, token) = GetUrlAndToken(options, input.Model, true);

            // 构建请求对象，启用流式模式
            var request = BuildClaudeRequest(input, true, isThink);

            var response = await HttpClientFactory.GetHttpClient(url).HttpRequestRaw(url, request, token, "Authorization");

            // 处理响应错误
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new ThorRateLimitException();
            }

            if (response.StatusCode >= HttpStatusCode.BadRequest)
            {
                logger.LogError("GCP流式对话异常, StatusCode: {StatusCode} Response: {Response} Url:{Url}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync(cancellationToken),
                    url);

                throw new Exception($"GCP流式对话异常，状态码：{response.StatusCode}");
            }

            // 处理流式响应
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            string? line = string.Empty;
            var first = true;
            while ((line = await reader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
            {
                line += Environment.NewLine;

                if (line.StartsWith('{'))
                {
                    logger.LogInformation("GCP对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                        line);

                    throw new Exception("GCP对话异常" + line);
                }

                if (line.StartsWith(OpenAIConstant.Data))
                    line = line[OpenAIConstant.Data.Length..];

                line = line.Trim();

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line == OpenAIConstant.Done)
                {
                    break;
                }

                if (line.StartsWith(':'))
                {
                    continue;
                }

                if (line.StartsWith("event: ", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var result = JsonSerializer.Deserialize<ClaudeStreamDto>(line,
                    ThorJsonSerializer.DefaultOptions);

                if (result?.type == "error")
                {
                    logger.LogInformation("GCP对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                       line);
                
                    throw new Exception("GCP对话异常" + line);
                }

                if (result?.type == "content_block_delta")
                {
                    if (result.delta.type is "text" or "text_delta")
                    {
                        yield return new ThorChatCompletionsResponse()
                        {
                            Choices =
                            [
                                new()
                            {
                                Message = new ThorChatMessage()
                                {
                                    Content = result.delta.text,
                                    Role = "assistant",
                                }
                            }
                            ],
                            Model = input.Model,
                            Id = result?.message?.id,
                            Usage = new ThorUsageResponse()
                            {
                                CompletionTokens = result?.message?.usage?.output_tokens,
                                PromptTokens = result?.message?.usage?.input_tokens,
                            }
                        };
                    }
                    else if (result.delta.type == "signature_delta")
                    {
                    }
                    else
                    {
                        yield return new ThorChatCompletionsResponse()
                        {
                            Choices = new List<ThorChatChoiceResponse>()
                        {
                            new()
                            {
                                Message = new ThorChatMessage()
                                {
                                    ReasoningContent = result.delta.thinking,
                                    Role = "assistant",
                                }
                            }
                        },
                            Model = input.Model,
                            Id = result?.message?.id,
                            Usage = new ThorUsageResponse()
                            {
                                CompletionTokens = result?.message?.usage?.output_tokens,
                                PromptTokens = result?.message?.usage?.input_tokens,
                            }
                        };
                    }

                    continue;
                }

                if (result.type == "message_start")
                {
                    yield return new ThorChatCompletionsResponse()
                    {
                        Choices =
                        [
                            new ThorChatChoiceResponse()
                        {
                            Message = new ThorChatMessage()
                            {
                                Content = result?.delta?.text??"",
                                Role = "assistant",
                            }
                        }
                        ],
                        Model = input.Model,
                        Usage = new ThorUsageResponse()
                        {
                            PromptTokens = result.message.usage.input_tokens,
                        }
                    };

                    continue;
                }

                if (result.type == "message_delta")
                {
                    yield return new ThorChatCompletionsResponse()
                    {
                        Choices =
                        [
                            new ThorChatChoiceResponse()
                        {
                            Message = new ThorChatMessage()
                            {
                                Content = result?.delta?.text??"",
                                Role = "assistant",
                            }
                        }
                        ],
                        Model = input.Model,
                        Usage = new ThorUsageResponse()
                        {
                            CompletionTokens = result.usage.output_tokens,
                        }
                    };

                    continue;
                }


                if (result.message == null)
                {
                    continue;
                }

                var chat = CreateResponse(result.message);

                var content = chat?.FirstOrDefault()?.Delta;

                if (first && string.IsNullOrWhiteSpace(content?.Content) && string.IsNullOrEmpty(content?.ReasoningContent))
                {
                    continue;
                }

                if (first && content.Content == OpenAIConstant.ThinkStart)
                {
                    isThink = true;
                    continue;
                    // 需要将content的内容转换到其他字段
                }

                if (isThink && content.Content.Contains(OpenAIConstant.ThinkEnd))
                {
                    isThink = false;
                    // 需要将content的内容转换到其他字段
                    continue;
                }

                if (isThink)
                {
                    // 需要将content的内容转换到其他字段
                    foreach (var choice in chat)
                    {
                        choice.Delta.ReasoningContent = choice.Delta.Content;
                        choice.Delta.Content = string.Empty;
                    }
                }

                first = false;

                yield return new ThorChatCompletionsResponse()
                {
                    Choices = chat,
                    Model = input.Model,
                    Id = result.message.id,
                    Usage = new ThorUsageResponse()
                    {
                        CompletionTokens = result.message.usage.output_tokens,
                        PromptTokens = result.message.usage.input_tokens,
                    }
                };
            }
        }

        #region 辅助方法

        /// <summary>
        /// 获取URL和令牌
        /// </summary>
        private (string url, string token) GetUrlAndToken(ThorPlatformOptions? options, string model, bool isStreaming)
        {
            var url = GCPClaudeFactory.GetAddress(options, model, isStreaming);
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("GCP 无效的url");
            }

            var token = $"Bearer {GCPClaudeFactory.GetToken(options)}";
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("GCP 无效的token");
            }

            return (url, token);
        }

        /// <summary>
        /// 构建Claude API请求对象
        /// </summary>
        private object BuildClaudeRequest(ThorChatCompletionsRequest input, bool isStreaming, bool isThink)

        {
            // 初始化默认值
            var budgetTokens = 1024; // 最小预算

            // 确保 MaxTokens 至少为 2048
            if (input.MaxTokens < 2048 && isThink)
            {
                input.MaxTokens = 2048;
            }

            // 根据 MaxTokens 动态计算 budgetTokens，适合文学任务
            if (input.MaxTokens != null && isThink)
            {
                // 文学任务通常需要更多的输出空间而非思考空间
                // 为思维预留总令牌的 1/4，给输出预留 3/4
                budgetTokens = Math.Max(1024, input.MaxTokens.Value / 4);

                // 对于小说任务，即使 MaxTokens 较大，思维预算也可以相对较小
                // 因为这类任务需要更多的输出令牌来生成或分析文本
                budgetTokens = Math.Min(budgetTokens, 4096);
                // 额外安全检查：确保 budgetTokens 不超过 MaxTokens 的 75%
                budgetTokens = Math.Min(budgetTokens, (int)(input.MaxTokens.Value * 0.75));
            }
            //工具
            object tool_choice;
            if (input.ToolChoice is not null && input.ToolChoice.Type == "auto")
            {
                tool_choice = new
                {
                    type = "auto",
                    disable_parallel_tool_use = false,
                };
            }
            else if (input.ToolChoice is not null && input.ToolChoice.Type == "any")
            {
                tool_choice = new
                {
                    type = "any",
                    disable_parallel_tool_use = false,
                };
            }
            else if (input.ToolChoice is not null && input.ToolChoice.Type == "tool")
            {
                tool_choice = new
                {
                    type = "tool",
                    name = input.ToolChoice.Function?.Name,
                    disable_parallel_tool_use = false,
                };
            }
            else
            {
                tool_choice = null;
            }

            // 提取系统提示词
            var system = input.Messages.Where(x => x.Role == "system").FirstOrDefault()?.Content ?? "";

            // 构建用户和助手的消息
            var userAssistantMessages = input.Messages.Where(x => x.Role != "system").ToList();

            // 构建消息格式
            var messages = new List<object>();
            foreach (var msg in userAssistantMessages)
            {
                if (msg.Role == "user")
                {
                    messages.Add(new
                    {
                        role = "user",
                        content = new[]
                        {
                            new
                            {
                                type = "text",
                                text = msg.Content
                            }
                        }
                    });
                }
                else if (msg.Role == "assistant")
                {
                    messages.Add(new
                    {
                        role = "assistant",
                        content = new[]
                        {
                            new
                            {
                                type = "text",
                                text = msg.Content
                            }
                        }
                    });
                }
            }

            // 基本请求对象
            var baseRequest = new Dictionary<string, object>
            {
                ["anthropic_version"] = "vertex-2023-10-16",
                ["stream"] = isStreaming,
                ["max_tokens"] = input.MaxTokens ?? 8000,
                ["messages"] = messages
            };
            // 添加系统提示词（如果有）
            if (!string.IsNullOrEmpty(system))
            {
                baseRequest["system"] = system;
            }
            if (tool_choice != null)
            {
                baseRequest["tool_choice"] = tool_choice;
            }
            if (input.Tools != null)
            {
                baseRequest["tools"] = input.Tools.Select(x => new
                {
                    name = x.Function?.Name,
                    description = x.Function?.Description,
                    input_schema = new
                    {
                        type = x.Function?.Parameters?.Type,
                        required = x.Function?.Parameters?.Required,
                        properties = x.Function?.Parameters?.Properties?.ToDictionary(y => y.Key, y => new
                        {
                            description = y.Value.Description,
                            type = y.Value.Type,
                            @enum = y.Value.Enum
                        })
                    }
                }).ToArray();
            }
            if (isThink)
            {
                baseRequest["thinking"] = new
                {
                    type = "enabled",
                    budget_tokens = budgetTokens,
                };
            }
            else
            {
                baseRequest["temperature"] = input.Temperature ?? 1;
                baseRequest["top_p"] = input.TopP ?? 1;
            }
            return baseRequest;
        }

        /// <summary>
        /// 处理响应错误
        /// </summary>
        private async Task HandleResponseErrors(HttpResponseMessage responseMessage, string url, CancellationToken cancellationToken)
        {
            // 如果限流则抛出限流异常
            if (responseMessage.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new ThorRateLimitException();
            }

            if (responseMessage.StatusCode >= HttpStatusCode.BadRequest)
            {
                var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                logger.LogError("GCP对话异常, StatusCode: {StatusCode} Response: {Response} Url:{Url}",
                    responseMessage.StatusCode,
                    errorContent,
                    url);

                throw new Exception($"GCP对话异常，状态码：{responseMessage.StatusCode}，错误：{errorContent}");
            }

            // 确保响应成功
            responseMessage.EnsureSuccessStatusCode();
        }

        #endregion

        public List<ThorChatChoiceResponse> CreateResponse(ClaudeChatCompletionDto completionDto)
        {
            var response = new ThorChatChoiceResponse();
            var chatMessage = new ThorChatMessage();
            if (completionDto == null)
            {
                return new List<ThorChatChoiceResponse>();
            }

            if (completionDto.content.Any(x => x.type.Equals("thinking", StringComparison.OrdinalIgnoreCase)))
            {
                // 将推理字段合并到返回对象去
                chatMessage.ReasoningContent = completionDto.content
                    .First(x => x.type.Equals("thinking", StringComparison.OrdinalIgnoreCase)).Thinking;

                chatMessage.Role = completionDto.role;
                chatMessage.Content = completionDto.content
                    .First(x => x.type.Equals("text", StringComparison.OrdinalIgnoreCase)).text;
            }
            else
            {
                chatMessage.Role = completionDto.role;
                chatMessage.Content = completionDto.content
                    .FirstOrDefault()?.text;
            }

            response.Delta = chatMessage;
            response.Message = chatMessage;

            return new List<ThorChatChoiceResponse> { response };
        }

        private object CreateMessage(List<ThorChatMessage> messages, ThorPlatformOptions options)
        {
            var list = new List<object>();

            foreach (var message in messages)
            {
                // 如果是图片
                if (message.ContentCalculated is IList<ThorChatMessageContent> contentCalculated)
                {
                    list.Add(new
                    {
                        role = message.Role,
                        content = (List<object>)contentCalculated.Select<ThorChatMessageContent, object>(x =>
                        {
                            if (x.Type == "text")
                            {
                                if (options.Other.Equals("true", StringComparison.OrdinalIgnoreCase))
                                {
                                    return new
                                    {
                                        type = "text",
                                        text = x.Text,
                                        cache_control = new
                                        {
                                            type = "ephemeral"
                                        }
                                    };
                                }

                                return new
                                {
                                    type = "text",
                                    text = x.Text
                                };
                            }

                            var isBase64 = x.ImageUrl?.Url.StartsWith("http") == true;

                            if (options.Other.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                return new
                                {
                                    type = "image",
                                    source = new
                                    {
                                        type = isBase64 ? "base64" : "url",
                                        media_type = "image/png",
                                        data = x.ImageUrl?.Url,
                                    },
                                    cache_control = new
                                    {
                                        type = "ephemeral"
                                    }
                                };
                            }

                            return new
                            {
                                type = "image",
                                source = new
                                {
                                    type = isBase64 ? "base64" : "url",
                                    media_type = "image/png",
                                    data = x.ImageUrl?.Url,
                                }
                            };
                        })
                    });
                }
                else
                {
                    if (options.Other.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (message.Role == "system")
                        {
                            list.Add(new
                            {
                                type = "text",
                                text = message.Content,
                                cache_control = new
                                {
                                    type = "ephemeral"
                                }
                            });
                        }
                        else
                        {
                            list.Add(new
                            {
                                role = message.Role,
                                content = message.Content
                            });
                        }
                    }
                    else
                    {
                        if (message.Role == "system")
                        {
                            list.Add(new
                            {
                                type = "text",
                                text = message.Content
                            });
                        }
                        else
                        {
                            list.Add(new
                            {
                                role = message.Role,
                                content = message.Content
                            });
                        }
                    }
                }
            }

            return list;
        }

    }
}
