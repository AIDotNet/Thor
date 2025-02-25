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

        var headers = new Dictionary<string, string>();

        var response = await client.PostJsonAsync(options.Address.TrimEnd('/') + "/v1/messages", new
        {
            model = input.Model,
            max_tokens = input.MaxTokens ?? 2000,
            messages = CreateMessage(input.Messages),
            top_p = input.TopP,
            thinking = input.Thinking,
            tools = input.Tools?.Select(x => new
            {
                name = x.Function?.Name,
                description = x.Function?.Description,
                type = x.Type,
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
            temperature = input.Temperature
        }, string.Empty, headers);

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new BusinessException("渠道未登录,请联系管理人员", "401");
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
        }

        var value =
            await response.Content.ReadFromJsonAsync<ClaudeChatCompletionDto>(ThorJsonSerializer.DefaultOptions,
                cancellationToken: cancellationToken);

        return new ThorChatCompletionsResponse()
        {
            Choices = CreateResponse(value),
            Model = input.Model,
            Id = value.id,
            Usage = new ThorUsageResponse()
            {
                CompletionTokens = value.usage.output_tokens,
                PromptTokens = value.usage.input_tokens,
            }
        };
    }

    public List<ThorChatChoiceResponse> CreateResponse(ClaudeChatCompletionDto completionDto)
    {
        var response = new ThorChatChoiceResponse();
        var chatMessage = new ThorChatMessage();
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

    private object CreateMessage(List<ThorChatMessage> messages)
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
                    content = contentCalculated.Select<ThorChatMessageContent, object>(x =>
                    {
                        if (x.Type == "text")
                        {
                            return new
                            {
                                type = "text",
                                text = x.Text
                            };
                        }

                        var isBase64 = x.ImageUrl?.Url.StartsWith("http") == true;

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
                list.Add(new
                {
                    type = "text",
                    text = message.Content
                });
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

        var headers = new Dictionary<string, string>();

        var response = await client.HttpRequestRaw(options.Address.TrimEnd('/') + "/v1/messages", new
        {
            model = input.Model,
            max_tokens = input.MaxTokens ?? 2000,
            messages = CreateMessage(input.Messages),
            top_p = input.TopP,
            thinking = input.Thinking,
            tools = input.Tools?.Select(x => new
            {
                name = x.Function?.Name,
                description = x.Function?.Description,
                type = x.Type,
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
            temperature = input.Temperature
        }, string.Empty, headers);

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/chat/completions");
        openai?.SetTag("Model", input.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new BusinessException("渠道未登录,请联系管理人员", "401");
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
        }

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        var first = true;
        var isThink = false;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogInformation("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    line);

                throw new BusinessException("OpenAI对话异常", line);
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


            var result = JsonSerializer.Deserialize<ClaudeChatCompletionDto>(line,
                ThorJsonSerializer.DefaultOptions);

            var chat = CreateResponse(result);

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
                Id = result.id,
                Usage = new ThorUsageResponse()
                {
                    CompletionTokens = result.usage.output_tokens,
                    PromptTokens = result.usage.input_tokens,
                }
            };
        }
    }
}