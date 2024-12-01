using Claudia;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using ThorChatCompletionsResponse =
    Thor.Abstractions.Chats.Dtos.ThorChatCompletionsResponse;

namespace Thor.Claude.Chats;

public sealed class ClaudiaChatCompletionsService : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = ClaudiaFactory.CreateClient(options.ApiKey, options.Address);
        var tools = new List<Tool>();

        if (input.Tools != null)
        {
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
        }

        var result = await anthropic.Messages.CreateAsync(new MessageRequest
        {
            Model = input.Model,
            MaxTokens = input.MaxTokens ?? 2000,
            Messages = input.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray(),
            TopP = input.TopP,
            Tools = tools.ToArray(),
            Temperature = input.Temperature,
        }, cancellationToken: cancellationToken);

        var toolsResult = new List<ThorToolCall>();

        if (result.Content.Any(x => string.IsNullOrEmpty(x.ToolResultId)))
        {
            foreach (var content in result.Content)
            {
                toolsResult.Add(new ThorToolCall()
                {
                    Id = content.ToolResultId,
                    Type = content.Type,
                    Function = new ThorChatMessageFunction()
                    {
                        Arguments = JsonSerializer.Serialize(content.ToolUseInput),
                        Name = content.ToolUseName
                    }
                });
            }
        }

        var message = ThorChatMessage.CreateAssistantMessage(result.Content.FirstOrDefault()?.Text ?? string.Empty, toolCalls: toolsResult);
        return new ThorChatCompletionsResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = message,
                    Message = message,
                    FinishReason = "stop",
                    Index = 0,

                }
            ],

            Model = input.Model
        };
    }

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest input, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = ClaudiaFactory.CreateClient(options.ApiKey, options.Address);
        var tools = new List<Tool>();

        if (input.Tools != null)
        {
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
        }

        await foreach (var result in anthropic.Messages.CreateStreamAsync(new MessageRequest
        {
            Model = input.Model,
            MaxTokens = input.MaxTokens ?? 2000,
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
                yield return new ThorChatCompletionsResponse()
                {
                    Choices =
                    [
                        new()
                        {
                            Delta = ThorChatMessage.CreateAssistantMessage(content.Delta.Text,toolCalls:content.Delta.ToolResultContent?.Select(
                                x =>
                                    new ThorToolCall()
                                    {
                                        Id = x.ToolResultId,
                                        Type = x.Type,
                                        Function = new ThorChatMessageFunction()
                                        {
                                            Name = x.ToolUseName,
                                            Arguments = x.ToolUseInput != null
                                                ? JsonSerializer.Serialize(x.ToolUseInput, null,
                                                    new JsonSerializerOptions
                                                    {
                                                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder
                                                            .UnsafeRelaxedJsonEscaping,
                                                    })
                                                : string.Empty
                                        },
                                    }).ToList()),
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