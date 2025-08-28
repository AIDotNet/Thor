using System.Text.Json;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.Abstractions.Anthropic;

public class AnthropicToOpenAI
{
    /// <summary>
    /// 将AnthropicInput转换为ThorChatCompletionsRequest
    /// </summary>
    public static ThorChatCompletionsRequest ConvertAnthropicToOpenAI(AnthropicInput anthropicInput)
    {
        var openAIRequest = new ThorChatCompletionsRequest
        {
            Model = anthropicInput.Model,
            MaxTokens = anthropicInput.MaxTokens,
            Stream = anthropicInput.Stream,
            Messages = new List<ThorChatMessage>(anthropicInput.Messages.Count)
        };

        // high medium minimal low
        if (openAIRequest.Model.EndsWith("-high") ||
            openAIRequest.Model.EndsWith("-medium") ||
            openAIRequest.Model.EndsWith("-minimal") ||
            openAIRequest.Model.EndsWith("-low"))
        {
            openAIRequest.ReasoningEffort = openAIRequest.Model switch
            {
                var model when model.EndsWith("-high") => "high",
                var model when model.EndsWith("-medium") => "medium",
                var model when model.EndsWith("-minimal") => "minimal",
                var model when model.EndsWith("-low") => "low",
                _ => "medium"
            };

            openAIRequest.Model = openAIRequest.Model.Replace("-high", "")
                .Replace("-medium", "")
                .Replace("-minimal", "")
                .Replace("-low", "");
        }

        if (openAIRequest.Model.EndsWith("-thinking"))
        {
            openAIRequest.EnableThinking = true;
            openAIRequest.Model = openAIRequest.Model.Replace("-thinking", "");
        }

        if (openAIRequest.Stream == true)
        {
            openAIRequest.StreamOptions = new ThorStreamOptions()
            {
                IncludeUsage = true,
            };
        }

        if (!string.IsNullOrEmpty(anthropicInput.System))
        {
            openAIRequest.Messages.Add(ThorChatMessage.CreateSystemMessage(anthropicInput.System));
        }

        if (anthropicInput.Systems?.Count > 0)
        {
            foreach (var systemContent in anthropicInput.Systems)
            {
                openAIRequest.Messages.Add(ThorChatMessage.CreateSystemMessage(systemContent.Text ?? string.Empty));
            }
        }


        // 处理messages
        if (anthropicInput.Messages != null)
        {
            foreach (var message in anthropicInput.Messages)
            {
                var thorMessages = ConvertAnthropicMessageToThor(message);
                // 需要过滤 空消息
                if (thorMessages.Count == 0)
                {
                    continue;
                }

                openAIRequest.Messages.AddRange(thorMessages);
            }

            openAIRequest.Messages = openAIRequest.Messages
                .Where(m => !string.IsNullOrEmpty(m.Content) || m.Contents?.Count > 0 || m.ToolCalls?.Count > 0 ||
                            !string.IsNullOrEmpty(m.ToolCallId))
                .ToList();
        }

        // 处理tools
        if (anthropicInput.Tools is { Count: > 0 })
        {
            openAIRequest.Tools = anthropicInput.Tools.Where(x => x.name != "web_search")
                .Select(ConvertAnthropicToolToThor).ToList();
        }

        // 判断是否存在web_search
        if (anthropicInput.Tools?.Any(x => x.name == "web_search") == true)
        {
            openAIRequest.WebSearchOptions = new ThorChatWebSearchOptions()
            {
            };
        }

        // 处理tool_choice
        if (anthropicInput.ToolChoice != null)
        {
            openAIRequest.ToolChoice = ConvertAnthropicToolChoiceToThor(anthropicInput.ToolChoice);
        }

        return openAIRequest;
    }

    /// <summary>
    /// 根据最后的内容块类型和OpenAI的完成原因确定Claude的停止原因
    /// </summary>
    public static string GetStopReasonByLastContentType(string? openAiFinishReason, string lastContentBlockType)
    {
        // 如果最后一个内容块是工具调用，优先返回tool_use
        if (lastContentBlockType == "tool_use")
        {
            return "tool_use";
        }

        // 否则使用标准的转换逻辑
        return GetClaudeStopReason(openAiFinishReason);
    }

    /// <summary>
    /// 创建message_start事件
    /// </summary>
    public static ClaudeStreamDto CreateMessageStartEvent(string messageId, string model)
    {
        return new ClaudeStreamDto
        {
            type = "message_start",
            message = new ClaudeChatCompletionDto
            {
                id = messageId,
                type = "message",
                role = "assistant",
                model = model,
                content = new ClaudeChatCompletionDtoContent[0],
                Usage = new ClaudeChatCompletionDtoUsage
                {
                    input_tokens = 0,
                    output_tokens = 0,
                    cache_creation_input_tokens = 0,
                    cache_read_input_tokens = 0
                }
            }
        };
    }

    /// <summary>
    /// 创建content_block_start事件
    /// </summary>
    public static ClaudeStreamDto CreateContentBlockStartEvent()
    {
        return new ClaudeStreamDto
        {
            type = "content_block_start",
            index = 0,
            content_block = new ClaudeChatCompletionDtoContent_block
            {
                type = "text",
                id = null,
                name = null
            }
        };
    }

    /// <summary>
    /// 创建thinking block start事件
    /// </summary>
    public static ClaudeStreamDto CreateThinkingBlockStartEvent()
    {
        return new ClaudeStreamDto
        {
            type = "content_block_start",
            index = 0,
            content_block = new ClaudeChatCompletionDtoContent_block
            {
                type = "thinking",
                id = null,
                name = null
            }
        };
    }

    /// <summary>
    /// 创建content_block_delta事件
    /// </summary>
    public static ClaudeStreamDto CreateContentBlockDeltaEvent(string text)
    {
        return new ClaudeStreamDto
        {
            type = "content_block_delta",
            index = 0,
            delta = new ClaudeChatCompletionDtoDelta
            {
                type = "text_delta",
                text = text
            }
        };
    }

    /// <summary>
    /// 创建thinking delta事件
    /// </summary>
    public static ClaudeStreamDto CreateThinkingBlockDeltaEvent(string thinking)
    {
        return new ClaudeStreamDto
        {
            type = "content_block_delta",
            index = 0,
            delta = new ClaudeChatCompletionDtoDelta
            {
                type = "thinking",
                thinking = thinking
            }
        };
    }

    /// <summary>
    /// 创建content_block_stop事件
    /// </summary>
    public static ClaudeStreamDto CreateContentBlockStopEvent()
    {
        return new ClaudeStreamDto
        {
            type = "content_block_stop",
            index = 0
        };
    }

    /// <summary>
    /// 创建message_delta事件
    /// </summary>
    public static ClaudeStreamDto CreateMessageDeltaEvent(string finishReason, ClaudeChatCompletionDtoUsage usage)
    {
        return new ClaudeStreamDto
        {
            type = "message_delta",
            Usage = usage,
            delta = new ClaudeChatCompletionDtoDelta
            {
                stop_reason = finishReason
            }
        };
    }

    /// <summary>
    /// 创建message_stop事件
    /// </summary>
    public static ClaudeStreamDto CreateMessageStopEvent()
    {
        return new ClaudeStreamDto
        {
            type = "message_stop"
        };
    }

    /// <summary>
    /// 创建tool block start事件
    /// </summary>
    public static ClaudeStreamDto CreateToolBlockStartEvent(string? toolId, string? toolName)
    {
        return new ClaudeStreamDto
        {
            type = "content_block_start",
            index = 0,
            content_block = new ClaudeChatCompletionDtoContent_block
            {
                type = "tool_use",
                id = toolId,
                name = toolName
            }
        };
    }

    /// <summary>
    /// 创建tool delta事件
    /// </summary>
    public static ClaudeStreamDto CreateToolBlockDeltaEvent(string partialJson)
    {
        return new ClaudeStreamDto
        {
            type = "content_block_delta",
            index = 0,
            delta = new ClaudeChatCompletionDtoDelta
            {
                type = "input_json_delta",
                partial_json = partialJson
            }
        };
    }

    /// <summary>
    /// 转换Anthropic消息为Thor消息列表
    /// </summary>
    public static List<ThorChatMessage> ConvertAnthropicMessageToThor(AnthropicMessageInput anthropicMessage)
    {
        var results = new List<ThorChatMessage>();

        // 处理简单的字符串内容
        if (anthropicMessage.Content != null)
        {
            var thorMessage = new ThorChatMessage
            {
                Role = anthropicMessage.Role,
                Content = anthropicMessage.Content
            };
            results.Add(thorMessage);
            return results;
        }

        // 处理多模态内容
        if (anthropicMessage.Contents is { Count: > 0 })
        {
            var currentContents = new List<ThorChatMessageContent>();
            var currentToolCalls = new List<ThorToolCall>();

            foreach (var content in anthropicMessage.Contents)
            {
                if (content.Type == "text")
                {
                    currentContents.Add(ThorChatMessageContent.CreateTextContent(content.Text ?? string.Empty));
                }
                else if (content.Type == "image")
                {
                    if (content.Source != null)
                    {
                        var imageUrl = content.Source.Type == "base64"
                            ? $"data:{content.Source.MediaType};base64,{content.Source.Data}"
                            : content.Source.Data;
                        currentContents.Add(ThorChatMessageContent.CreateImageUrlContent(imageUrl ?? string.Empty));
                    }
                }
                else if (content.Type == "tool_use")
                {
                    // 如果有普通内容，先创建内容消息
                    if (currentContents.Count > 0)
                    {
                        if (currentContents.Count == 1 && currentContents.Any(x => x.Type == "text"))
                        {
                            var contentMessage = new ThorChatMessage
                            {
                                Role = anthropicMessage.Role,
                                ContentCalculated = currentContents.FirstOrDefault()?.Text ?? string.Empty
                            };
                            results.Add(contentMessage);
                        }
                        else
                        {
                            var contentMessage = new ThorChatMessage
                            {
                                Role = anthropicMessage.Role,
                                Contents = currentContents
                            };
                            results.Add(contentMessage);
                        }

                        currentContents = new List<ThorChatMessageContent>();
                    }

                    // 收集工具调用
                    var toolCall = new ThorToolCall
                    {
                        Id = content.Id,
                        Type = "function",
                        Function = new ThorChatMessageFunction
                        {
                            Name = content.Name,
                            Arguments = JsonSerializer.Serialize(content.Input)
                        }
                    };
                    currentToolCalls.Add(toolCall);
                }
                else if (content.Type == "tool_result")
                {
                    // 如果有普通内容，先创建内容消息
                    if (currentContents.Count > 0)
                    {
                        var contentMessage = new ThorChatMessage
                        {
                            Role = anthropicMessage.Role,
                            Contents = currentContents
                        };
                        results.Add(contentMessage);
                        currentContents = new List<ThorChatMessageContent>();
                    }

                    // 如果有工具调用，先创建工具调用消息
                    if (currentToolCalls.Count > 0)
                    {
                        var toolCallMessage = new ThorChatMessage
                        {
                            Role = anthropicMessage.Role,
                            ToolCalls = currentToolCalls
                        };
                        results.Add(toolCallMessage);
                        currentToolCalls = new List<ThorToolCall>();
                    }

                    // 创建工具结果消息
                    var toolMessage = new ThorChatMessage
                    {
                        Role = "tool",
                        ToolCallId = content.ToolUseId,
                        Content = content.Content?.ToString() ?? string.Empty
                    };
                    results.Add(toolMessage);
                }
            }

            // 处理剩余的内容
            if (currentContents.Count > 0)
            {
                var contentMessage = new ThorChatMessage
                {
                    Role = anthropicMessage.Role,
                    Contents = currentContents
                };
                results.Add(contentMessage);
            }

            // 处理剩余的工具调用
            if (currentToolCalls.Count > 0)
            {
                var toolCallMessage = new ThorChatMessage
                {
                    Role = anthropicMessage.Role,
                    ToolCalls = currentToolCalls
                };
                results.Add(toolCallMessage);
            }
        }

        // 如果没有任何内容，返回一个空的消息
        if (results.Count == 0)
        {
            results.Add(new ThorChatMessage
            {
                Role = anthropicMessage.Role,
                Content = string.Empty
            });
        }

        // 如果只有一个text则使用content字段
        if (results is [{ Contents.Count: 1 }] &&
            results.FirstOrDefault()?.Contents?.FirstOrDefault()?.Type == "text" &&
            !string.IsNullOrEmpty(results.FirstOrDefault()?.Contents?.FirstOrDefault()?.Text))
        {
            return
            [
                new ThorChatMessage
                {
                    Role = results[0].Role,
                    Content = results.FirstOrDefault()?.Contents?.FirstOrDefault()?.Text ?? string.Empty
                }
            ];
        }

        return results;
    }

    /// <summary>
    /// 转换Anthropic工具为Thor工具
    /// </summary>
    public static ThorToolDefinition ConvertAnthropicToolToThor(AnthropicMessageTool anthropicTool)
    {
        IDictionary<string, ThorToolFunctionPropertyDefinition> values =
            new Dictionary<string, ThorToolFunctionPropertyDefinition>();

        if (anthropicTool.InputSchema?.Properties != null)
        {
            foreach (var property in anthropicTool.InputSchema.Properties)
            {
                var propertyValueStr = property.Value?.ToString();
                if (propertyValueStr != null)
                {
                    var propertyDefinition =
                        JsonSerializer.Deserialize<ThorToolFunctionPropertyDefinition>(propertyValueStr);
                    if (propertyDefinition != null)
                    {
                        values.Add(property.Key, propertyDefinition);
                    }
                }
            }
        }


        return new ThorToolDefinition
        {
            Type = "function",
            Function = new ThorToolFunctionDefinition
            {
                Name = anthropicTool.name,
                Description = anthropicTool.Description,
                Parameters = new ThorToolFunctionPropertyDefinition
                {
                    Type = anthropicTool.InputSchema?.Type ?? "object",
                    Properties = values,
                    Required = anthropicTool.InputSchema?.Required?.ToList() ?? new List<string>()
                }
            }
        };
    }

    /// <summary>
    /// 将OpenAI的完成原因转换为Claude的停止原因
    /// </summary>
    public static string GetClaudeStopReason(string? openAIFinishReason)
    {
        return openAIFinishReason switch
        {
            "stop" => "end_turn",
            "length" => "max_tokens",
            "tool_calls" => "tool_use",
            "content_filter" => "stop_sequence",
            _ => "end_turn"
        };
    }

    /// <summary>
    /// 将OpenAI响应转换为Claude响应格式
    /// </summary>
    public static ClaudeChatCompletionDto ConvertOpenAIToClaude(ThorChatCompletionsResponse openAIResponse,
        AnthropicInput originalRequest)
    {
        var claudeResponse = new ClaudeChatCompletionDto
        {
            id = openAIResponse.Id,
            type = "message",
            role = "assistant",
            model = openAIResponse.Model ?? originalRequest.Model,
            stop_reason = GetClaudeStopReason(openAIResponse.Choices?.FirstOrDefault()?.FinishReason),
            stop_sequence = "",
            content = new ClaudeChatCompletionDtoContent[0]
        };

        if (openAIResponse.Choices != null && openAIResponse.Choices.Count > 0)
        {
            var choice = openAIResponse.Choices.First();
            var contents = new List<ClaudeChatCompletionDtoContent>();

            // 处理思维内容
            if (!string.IsNullOrEmpty(choice.Message.ReasoningContent))
            {
                contents.Add(new ClaudeChatCompletionDtoContent
                {
                    type = "thinking",
                    Thinking = choice.Message.ReasoningContent
                });
            }

            // 处理文本内容
            if (!string.IsNullOrEmpty(choice.Message.Content))
            {
                contents.Add(new ClaudeChatCompletionDtoContent
                {
                    type = "text",
                    text = choice.Message.Content
                });
            }

            // 处理工具调用
            if (choice.Message.ToolCalls != null && choice.Message.ToolCalls.Count > 0)
            {
                foreach (var toolCall in choice.Message.ToolCalls)
                {
                    contents.Add(new ClaudeChatCompletionDtoContent
                    {
                        type = "tool_use",
                        id = toolCall.Id,
                        name = toolCall.Function?.Name,
                        input = JsonSerializer.Deserialize<object>(toolCall.Function?.Arguments ?? "{}")
                    });
                }
            }

            claudeResponse.content = contents.ToArray();
        }

        // 处理使用情况统计
        if (openAIResponse.Usage != null)
        {
            claudeResponse.Usage = new ClaudeChatCompletionDtoUsage
            {
                input_tokens = openAIResponse.Usage.PromptTokens,
                output_tokens = (int?)openAIResponse.Usage.CompletionTokens,
                cache_read_input_tokens = openAIResponse.Usage.PromptTokensDetails?.CachedTokens
            };
        }

        return claudeResponse;
    }


    /// <summary>
    /// 转换Anthropic工具选择为Thor工具选择
    /// </summary>
    public static ThorToolChoice ConvertAnthropicToolChoiceToThor(AnthropicTooChoiceInput anthropicToolChoice)
    {
        return new ThorToolChoice
        {
            Type = anthropicToolChoice.Type ?? "auto",
            Function = anthropicToolChoice.Name != null
                ? new ThorToolChoiceFunctionTool { Name = anthropicToolChoice.Name }
                : null
        };
    }
}