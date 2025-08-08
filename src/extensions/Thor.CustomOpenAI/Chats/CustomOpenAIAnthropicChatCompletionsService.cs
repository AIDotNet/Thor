using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Anthropic;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;

namespace Thor.CustomOpenAI.Chats;

/// <summary>
/// OpenAI到Claude适配器服务
/// 将Claude格式的请求转换为OpenAI格式，然后将OpenAI的响应转换为Claude格式
/// </summary>
public class CustomOpenAIAnthropicChatCompletionsService : IAnthropicChatCompletionsService
{
    private readonly IThorChatCompletionsService _openAIChatService;
    private readonly ILogger<CustomOpenAIAnthropicChatCompletionsService> _logger;

    public CustomOpenAIAnthropicChatCompletionsService(
        IThorChatCompletionsService openAIChatService,
        ILogger<CustomOpenAIAnthropicChatCompletionsService> logger)
    {
        _openAIChatService = openAIChatService;
        _logger = logger;
    }

    /// <summary>
    /// 非流式对话补全
    /// </summary>
    public async Task<ClaudeChatCompletionDto> ChatCompletionsAsync(AnthropicInput request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 转换请求格式：Claude -> OpenAI
            var openAIRequest = AnthropicToOpenAI.ConvertAnthropicToOpenAI(request);

            if (openAIRequest.Model.StartsWith("gpt-5"))
            {
                openAIRequest.MaxCompletionTokens = request.MaxTokens;
                openAIRequest.MaxTokens = null;
            }
            else if (openAIRequest.Model.StartsWith("o3-mini") || openAIRequest.Model.StartsWith("o4-mini"))
            {
                openAIRequest.MaxCompletionTokens = request.MaxTokens;
                openAIRequest.MaxTokens = null;
                openAIRequest.Temperature = null;
            }
            // 调用OpenAI服务
            var openAIResponse =
                await _openAIChatService.ChatCompletionsAsync(openAIRequest, options, cancellationToken);

            // 转换响应格式：OpenAI -> Claude
            var claudeResponse = AnthropicToOpenAI.ConvertOpenAIToClaude(openAIResponse, request);

            return claudeResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI到Claude适配器异常");
            throw;
        }
    }

    /// <summary>
    /// 流式对话补全
    /// </summary>
    public async IAsyncEnumerable<(string, string, ClaudeStreamDto?)> StreamChatCompletionsAsync(AnthropicInput request,
        ThorPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var openAIRequest = AnthropicToOpenAI.ConvertAnthropicToOpenAI(request);
        openAIRequest.Stream = true;

        if (openAIRequest.Model.StartsWith("gpt-5"))
        {
            openAIRequest.MaxCompletionTokens = request.MaxTokens;
            openAIRequest.MaxTokens = null;
        }
        else if (openAIRequest.Model.StartsWith("o3-mini") || openAIRequest.Model.StartsWith("o4-mini"))
        {
            openAIRequest.MaxCompletionTokens = request.MaxTokens;
            openAIRequest.MaxTokens = null;
            openAIRequest.Temperature = null;
        }
        var messageId = Guid.NewGuid().ToString();
        var hasStarted = false;
        var hasTextContentBlockStarted = false;
        var hasThinkingContentBlockStarted = false;
        var toolBlocksStarted = new Dictionary<int, bool>(); // 使用索引而不是ID
        var toolCallIds = new Dictionary<int, string>(); // 存储每个索引对应的ID
        var toolCallIndexToBlockIndex = new Dictionary<int, int>(); // 工具调用索引到块索引的映射
        var accumulatedUsage = new ClaudeChatCompletionDtoUsage();
        var isFinished = false;
        var currentContentBlockType = ""; // 跟踪当前内容块类型
        var currentBlockIndex = 0; // 跟踪当前块索引
        var lastContentBlockType = ""; // 跟踪最后一个内容块类型，用于确定停止原因

        await foreach (var openAIResponse in _openAIChatService.StreamChatCompletionsAsync(openAIRequest, options,
                           cancellationToken))
        {
            // 发送message_start事件
            if (!hasStarted && openAIResponse.Choices?.Count > 0 &&
                openAIResponse.Choices.Any(x => x.Delta.ToolCalls?.Count > 0) == false)
            {
                hasStarted = true;
                var messageStartEvent = AnthropicToOpenAI.CreateMessageStartEvent(messageId, request.Model);
                yield return ("message_start",
                    JsonSerializer.Serialize(messageStartEvent, ThorJsonSerializer.DefaultOptions), messageStartEvent);
            }

            // 更新使用情况统计
            if (openAIResponse.Usage != null)
            {
                // 使用最新的token计数（OpenAI通常在最后的响应中提供完整的统计）
                if (openAIResponse.Usage.PromptTokens.HasValue)
                {
                    accumulatedUsage.input_tokens = openAIResponse.Usage.PromptTokens.Value;
                }

                if (openAIResponse.Usage.CompletionTokens.HasValue)
                {
                    accumulatedUsage.output_tokens = (int)openAIResponse.Usage.CompletionTokens.Value;
                }

                if (openAIResponse.Usage.PromptTokensDetails?.CachedTokens.HasValue == true)
                {
                    accumulatedUsage.cache_read_input_tokens =
                        openAIResponse.Usage.PromptTokensDetails.CachedTokens.Value;
                }

                // 记录调试信息
                _logger.LogDebug("OpenAI Usage更新: Input={InputTokens}, Output={OutputTokens}, CacheRead={CacheRead}",
                    accumulatedUsage.input_tokens, accumulatedUsage.output_tokens,
                    accumulatedUsage.cache_read_input_tokens);
            }

            if (openAIResponse.Choices is { Count: > 0 })
            {
                var choice = openAIResponse.Choices.First();

                // 处理内容
                if (!string.IsNullOrEmpty(choice.Delta?.Content))
                {
                    // 如果当前有其他类型的内容块在运行，先结束它们
                    if (currentContentBlockType != "text" && !string.IsNullOrEmpty(currentContentBlockType))
                    {
                        var stopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                        stopEvent.index = currentBlockIndex;
                        yield return ("content_block_stop",
                            JsonSerializer.Serialize(stopEvent, ThorJsonSerializer.DefaultOptions), stopEvent);
                        currentBlockIndex++; // 切换内容块时增加索引
                        currentContentBlockType = "";
                    }

                    // 发送content_block_start事件（仅第一次）
                    if (!hasTextContentBlockStarted || currentContentBlockType != "text")
                    {
                        hasTextContentBlockStarted = true;
                        currentContentBlockType = "text";
                        lastContentBlockType = "text";
                        var contentBlockStartEvent = AnthropicToOpenAI.CreateContentBlockStartEvent();
                        contentBlockStartEvent.index = currentBlockIndex;
                        yield return ("content_block_start",
                            JsonSerializer.Serialize(contentBlockStartEvent, ThorJsonSerializer.DefaultOptions),
                            contentBlockStartEvent);
                    }

                    // 发送content_block_delta事件
                    var contentDeltaEvent = AnthropicToOpenAI.CreateContentBlockDeltaEvent(choice.Delta.Content);
                    contentDeltaEvent.index = currentBlockIndex;
                    yield return ("content_block_delta",
                        JsonSerializer.Serialize(contentDeltaEvent, ThorJsonSerializer.DefaultOptions),
                        contentDeltaEvent);
                }

                // 处理工具调用
                if (choice.Delta?.ToolCalls is { Count: > 0 })
                {
                    foreach (var toolCall in choice.Delta.ToolCalls)
                    {
                        var toolCallIndex = toolCall.Index; // 使用索引来标识工具调用

                        // 发送tool_use content_block_start事件
                        if (toolBlocksStarted.TryAdd(toolCallIndex, true))
                        {
                            // 如果当前有文本或thinking内容块在运行，先结束它们
                            if (currentContentBlockType == "text" || currentContentBlockType == "thinking")
                            {
                                var stopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                                stopEvent.index = currentBlockIndex;
                                yield return ("content_block_stop",
                                    JsonSerializer.Serialize(stopEvent, ThorJsonSerializer.DefaultOptions), stopEvent);
                                currentBlockIndex++; // 增加块索引
                            }
                            // 如果当前有其他工具调用在运行，也需要结束它们
                            else if (currentContentBlockType == "tool_use")
                            {
                                var stopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                                stopEvent.index = currentBlockIndex;
                                yield return ("content_block_stop",
                                    JsonSerializer.Serialize(stopEvent, ThorJsonSerializer.DefaultOptions), stopEvent);
                                currentBlockIndex++; // 增加块索引
                            }

                            currentContentBlockType = "tool_use";
                            lastContentBlockType = "tool_use";

                            // 为此工具调用分配一个新的块索引
                            toolCallIndexToBlockIndex[toolCallIndex] = currentBlockIndex;

                            // 保存工具调用的ID（如果有的话）
                            if (!string.IsNullOrEmpty(toolCall.Id))
                            {
                                toolCallIds[toolCallIndex] = toolCall.Id;
                            }
                            else if (!toolCallIds.ContainsKey(toolCallIndex))
                            {
                                // 如果没有ID且之前也没有保存过，生成一个新的ID
                                toolCallIds[toolCallIndex] = Guid.NewGuid().ToString();
                            }

                            var toolBlockStartEvent = AnthropicToOpenAI.CreateToolBlockStartEvent(
                                toolCallIds[toolCallIndex],
                                toolCall.Function?.Name);
                            toolBlockStartEvent.index = currentBlockIndex;
                            yield return ("content_block_start",
                                JsonSerializer.Serialize(toolBlockStartEvent, ThorJsonSerializer.DefaultOptions),
                                toolBlockStartEvent);
                        }

                        // 如果有增量的参数，发送content_block_delta事件
                        if (!string.IsNullOrEmpty(toolCall.Function?.Arguments))
                        {
                            var toolDeltaEvent = AnthropicToOpenAI.CreateToolBlockDeltaEvent(toolCall.Function.Arguments);
                            // 使用该工具调用对应的块索引
                            toolDeltaEvent.index = toolCallIndexToBlockIndex[toolCallIndex];
                            yield return ("content_block_delta",
                                JsonSerializer.Serialize(toolDeltaEvent, ThorJsonSerializer.DefaultOptions),
                                toolDeltaEvent);
                        }
                    }
                }

                // 处理推理内容
                if (!string.IsNullOrEmpty(choice.Delta?.ReasoningContent))
                {
                    // 如果当前有其他类型的内容块在运行，先结束它们
                    if (currentContentBlockType != "thinking" && !string.IsNullOrEmpty(currentContentBlockType))
                    {
                        var stopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                        stopEvent.index = currentBlockIndex;
                        yield return ("content_block_stop",
                            JsonSerializer.Serialize(stopEvent, ThorJsonSerializer.DefaultOptions), stopEvent);
                        currentBlockIndex++; // 增加块索引
                        currentContentBlockType = "";
                    }

                    // 对于推理内容，也需要发送对应的事件
                    if (!hasThinkingContentBlockStarted || currentContentBlockType != "thinking")
                    {
                        hasThinkingContentBlockStarted = true;
                        currentContentBlockType = "thinking";
                        lastContentBlockType = "thinking";
                        var thinkingBlockStartEvent = AnthropicToOpenAI.CreateThinkingBlockStartEvent();
                        thinkingBlockStartEvent.index = currentBlockIndex;
                        yield return ("content_block_start",
                            JsonSerializer.Serialize(thinkingBlockStartEvent, ThorJsonSerializer.DefaultOptions),
                            thinkingBlockStartEvent);
                    }

                    var thinkingDeltaEvent = AnthropicToOpenAI.CreateThinkingBlockDeltaEvent(choice.Delta.ReasoningContent);
                    thinkingDeltaEvent.index = currentBlockIndex;
                    yield return ("content_block_delta",
                        JsonSerializer.Serialize(thinkingDeltaEvent, ThorJsonSerializer.DefaultOptions),
                        thinkingDeltaEvent);
                }

                // 处理结束
                if (!string.IsNullOrEmpty(choice.FinishReason) && !isFinished)
                {
                    isFinished = true;

                    // 发送content_block_stop事件（如果有活跃的内容块）
                    if (!string.IsNullOrEmpty(currentContentBlockType))
                    {
                        var contentBlockStopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                        contentBlockStopEvent.index = currentBlockIndex;
                        yield return ("content_block_stop",
                            JsonSerializer.Serialize(contentBlockStopEvent, ThorJsonSerializer.DefaultOptions),
                            contentBlockStopEvent);
                    }

                    // 发送message_delta事件
                    var messageDeltaEvent = AnthropicToOpenAI.CreateMessageDeltaEvent(
                        AnthropicToOpenAI.GetStopReasonByLastContentType(choice.FinishReason, lastContentBlockType), accumulatedUsage);

                    // 记录最终Usage统计
                    _logger.LogDebug(
                        "流式响应结束，最终Usage: Input={InputTokens}, Output={OutputTokens}, CacheRead={CacheRead}",
                        accumulatedUsage.input_tokens, accumulatedUsage.output_tokens,
                        accumulatedUsage.cache_read_input_tokens);

                    yield return ("message_delta",
                        JsonSerializer.Serialize(messageDeltaEvent, ThorJsonSerializer.DefaultOptions),
                        messageDeltaEvent);

                    // 发送message_stop事件
                    var messageStopEvent = AnthropicToOpenAI.CreateMessageStopEvent();
                    yield return ("message_stop",
                        JsonSerializer.Serialize(messageStopEvent, ThorJsonSerializer.DefaultOptions),
                        messageStopEvent);
                }
            }
        }

        // 确保流正确结束
        if (!isFinished)
        {
            if (!string.IsNullOrEmpty(currentContentBlockType))
            {
                var contentBlockStopEvent = AnthropicToOpenAI.CreateContentBlockStopEvent();
                contentBlockStopEvent.index = currentBlockIndex;
                yield return ("content_block_stop",
                    JsonSerializer.Serialize(contentBlockStopEvent, ThorJsonSerializer.DefaultOptions),
                    contentBlockStopEvent);
            }

            var messageDeltaEvent =
                AnthropicToOpenAI.CreateMessageDeltaEvent(AnthropicToOpenAI.GetStopReasonByLastContentType("end_turn", lastContentBlockType),
                    accumulatedUsage);
            yield return ("message_delta",
                JsonSerializer.Serialize(messageDeltaEvent, ThorJsonSerializer.DefaultOptions), messageDeltaEvent);

            var messageStopEvent = AnthropicToOpenAI.CreateMessageStopEvent();
            yield return ("message_stop", JsonSerializer.Serialize(messageStopEvent, ThorJsonSerializer.DefaultOptions),
                messageStopEvent);
        }
    }

}