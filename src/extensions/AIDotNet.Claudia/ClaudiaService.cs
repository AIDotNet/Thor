using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Claudia;
using OpenAI.ObjectModels.RequestModels;
using ChatCompletionCreateResponse = AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ChatCompletionCreateResponse;

namespace AIDotNet.Claudia;

public sealed class ClaudiaService : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = new Anthropic
        {
            ApiKey = options.Key,
        };
        if (!string.IsNullOrEmpty(options.Address))
        {
            anthropic.HttpClient.BaseAddress = new Uri(options.Address);
        }

        var tools = new List<Tool>();

        foreach (var tool in input.Tools)
        {
            var properties = new Dictionary<string, ToolProperty>();
            foreach (var definition in tool.Function.Parameters.Properties)
            {
                properties.Add(definition.Key, new ToolProperty()
                {
                    Description = definition.Value.Description ?? string.Empty,
                    Type = definition.Value.Type,
                    Enum = definition.Value.Enum?.ToArray(),
                });
            }

            tools.Add(new Tool
            {
                Name = tool.Function?.Name ?? string.Empty,
                Description = tool.Function?.Description ?? string.Empty,
                InputSchema = new InputSchema()
                {
                    Type = tool.Type,
                    Required = tool.Function?.Parameters?.Required?.ToArray(),
                    Properties = properties,
                }
            });
        }

        var result = await anthropic.Messages.CreateAsync(new MessageRequest
        {
            Model = input.Model,
            MaxTokens = (int)input.MaxTokens,
            Messages = input.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray(),
            TopP = input.TopP,
            Tools = tools.ToArray(),
            Temperature = input.Temperature,
        }, cancellationToken: cancellationToken);

        var toolsResult = new List<ToolCall>();

        foreach (var content in result.Content)
        {
            toolsResult.Add(new ToolCall()
            {
                Id = content.ToolResultId,
                Type = content.Type,
                FunctionCall = new FunctionCall()
                {
                    Arguments = JsonSerializer.Serialize(content.ToolUseInput),
                    Name = content.ToolUseName
                }
            });
        }

        return new ChatCompletionCreateResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = new ChatMessage("assistant", result.Content.FirstOrDefault()?.Text ?? string.Empty, null,
                        toolsResult),
                    FinishReason = "stop",
                    Index = 0,
                    Message = new ChatMessage("assistant", result.Content.FirstOrDefault()?.Text ?? string.Empty, null,
                        toolsResult),
                }
            ],

            Model = input.Model
        };
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = new Anthropic
        {
            ApiKey = options.Key,
        };
        if (!string.IsNullOrEmpty(options.Address))
        {
            anthropic.HttpClient.BaseAddress = new Uri(options.Address);
        }

        var tools = new List<Tool>();

        foreach (var tool in input.Tools)
        {
            var properties = new Dictionary<string, ToolProperty>();
            foreach (var definition in tool.Function.Parameters.Properties)
            {
                properties.Add(definition.Key, new ToolProperty()
                {
                    Description = definition.Value.Description ?? string.Empty,
                    Type = definition.Value.Type,
                    Enum = definition.Value.Enum?.ToArray(),
                });
            }

            tools.Add(new Tool
            {
                Name = tool.Function?.Name ?? string.Empty,
                Description = tool.Function?.Description ?? string.Empty,
                InputSchema = new InputSchema()
                {
                    Type = tool.Type,
                    Required = tool.Function?.Parameters?.Required?.ToArray(),
                    Properties = properties,
                }
            });
        }

        await foreach (var result in anthropic.Messages.CreateStreamAsync(new MessageRequest
                       {
                           Model = input.Model,
                           MaxTokens = (int)input.MaxTokens,
                           Messages = input.Messages.Select(x => new Message
                           {
                               Content = x.Content,
                               Role = x.Role
                           }).ToArray(),
                           TopP = input.TopP,
                           Tools = tools.ToArray(),
                           Temperature = input.Temperature,
                       }, cancellationToken: cancellationToken))
        {
            if (result is ContentBlockDelta content)
            {
                yield return new ChatCompletionCreateResponse()
                {
                    Choices =
                    [
                        new()
                        {
                            Delta = new ChatMessage("assistant", content.Delta.Text,null),
                            FinishReason = "stop",
                            Index = 0,
                        }
                    ],
                    Model = input.Model
                };
            }
        }
    }
}