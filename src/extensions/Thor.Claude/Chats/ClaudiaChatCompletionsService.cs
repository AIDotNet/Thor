using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.Claude.Chats.Dto;
using ThorChatCompletionsResponse =
    Thor.Abstractions.Chats.Dtos.ThorChatCompletionsResponse;

namespace Thor.Claude.Chats;

public sealed class ClaudiaChatCompletionsService(ILogger<ClaudiaChatCompletionsService> logger)
    : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Claudia 对话补全");

        if (string.IsNullOrEmpty(options.Address))
        {
            options.Address = "https://api.anthropic.com/";
        }

        var client = HttpClientFactory.GetHttpClient(options.Address);

        var headers = new Dictionary<string, string>
        {
            { "x-api-key", options.ApiKey },
            { "anthropic-version", "2023-06-01" }
        };

        bool isThink = input.Model.EndsWith("-thinking");
        input.Model = input.Model.Replace("-thinking", string.Empty);

        var budgetTokens = 1024;
        if (input.MaxTokens is < 2048)
        {
            input.MaxTokens = 2048;
        }

        if (input.MaxTokens != null && input.MaxTokens / 2 < 1024)
        {
            budgetTokens = input.MaxTokens.Value / (4 * 3);
        }

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

        // budgetTokens最大4096
        budgetTokens = Math.Min(budgetTokens, 4096);

        var response = await client.PostJsonAsync(options.Address.TrimEnd('/') + "/v1/messages", new
        {
            model = input.Model,
            max_tokens = input.MaxTokens ?? 2000,
            system = CreateMessage(input.Messages.Where(x => x.Role == "system").ToList(), options),
            messages = CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options),
            top_p = isThink ? null : input.TopP,
            tool_choice,
            thinking = isThink
                ? new
                {
                    type = "enabled",
                    budget_tokens = budgetTokens,
                }
                : null,
            tools = input.Tools?.Select(x => new
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
            }).ToArray(),
            temperature = isThink ? null : input.Temperature
        }, string.Empty, headers);

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new Exception("OpenAI对话异常" + response.StatusCode.ToString());
        }

        var value =
            await response.Content.ReadFromJsonAsync<ClaudeChatCompletionDto>(ThorJsonSerializer.DefaultOptions,
                cancellationToken: cancellationToken);

        var thor = new ThorChatCompletionsResponse()
        {
            Choices = CreateResponse(value),
            Model = input.Model,
            Id = value.id,
            Usage = new ThorUsageResponse()
            {
                CompletionTokens = value.usage.output_tokens,
                PromptTokens = value.usage.input_tokens
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

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest input, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("Claudia 对话补全");

        if (string.IsNullOrEmpty(options.Address))
        {
            options.Address = "https://api.anthropic.com/";
        }

        var client = HttpClientFactory.GetHttpClient(options.Address);

        var headers = new Dictionary<string, string>
        {
            { "x-api-key", options.ApiKey },
            { "anthropic-version", "2023-06-01" }
        };

        var isThinking = input.Model.EndsWith("thinking");
        input.Model = input.Model.Replace("-thinking", string.Empty);
        var budgetTokens = 1024;
        
        if (input.MaxTokens is < 2048)
        {
            input.MaxTokens = 2048;
        }

        if (input.MaxTokens != null && input.MaxTokens / 2 < 1024)
        {
            budgetTokens = input.MaxTokens.Value / (4 * 3);
        }

        // budgetTokens最大4096
        budgetTokens = Math.Min(budgetTokens, 4096);

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

        var response = await client.HttpRequestRaw(options.Address.TrimEnd('/') + "/v1/messages", new
        {
            model = input.Model,
            max_tokens = input.MaxTokens ?? 2048,
            stream = true,
            tool_choice,
            system = CreateMessage(input.Messages.Where(x => x.Role == "system").ToList(), options),
            messages = CreateMessage(input.Messages.Where(x => x.Role != "system").ToList(), options),
            top_p = isThinking ? null : input.TopP,
            thinking = isThinking
                ? new
                {
                    type = "enabled",
                    budget_tokens = budgetTokens,
                }
                : null,
            tools = input.Tools?.Select(x => new
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
            }).ToArray(),
            temperature = isThinking ? null : input.Temperature
        }, string.Empty, headers);

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new Exception("OpenAI对话异常" + response.StatusCode);
        }

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        var first = true;
        var isThink = false;
        while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogInformation("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    line);

                throw new Exception("OpenAI对话异常" + line);
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
                                Content = result.delta.text,
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
                                Content = result.delta.text,
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
}