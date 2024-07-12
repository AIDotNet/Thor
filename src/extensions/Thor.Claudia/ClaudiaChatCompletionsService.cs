using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Abstractions;
using Thor.Abstractions.Dto;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Claudia;
using OpenAI.ObjectModels.RequestModels;
using ChatCompletionCreateResponse =
    Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ChatCompletionCreateResponse;

namespace Thor.Claudia;

public sealed class ClaudiaChatCompletionsService : IChatCompletionsService
{
    public async Task<ChatCompletionCreateResponse> ChatCompletionsAsync(ChatCompletionsRequest input,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = AnthropicFactory.CreateClient(options.ApiKey, options.Address);
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
            MaxTokens = (int)(input.MaxTokens ?? 2000),
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

        if (result.Content.Any(x => string.IsNullOrEmpty(x.ToolResultId)))
        {
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

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatCompletionsAsync(
        ChatCompletionsRequest input, ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var anthropic = AnthropicFactory.CreateClient(options.ApiKey, options.Address);
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
                           MaxTokens = (int)(input.MaxTokens ?? 2000),
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
                            Delta = new("assistant", content.Delta.Text, null, content.Delta.ToolResultContent?.Select(
                                x =>
                                    new ToolCall()
                                    {
                                        Id = x.ToolResultId,
                                        Type = x.Type,
                                        FunctionCall = new FunctionCall()
                                        {
                                            Arguments = x.ToolUseInput != null
                                                ? JsonSerializer.Serialize(x.ToolUseInput, null,
                                                    new JsonSerializerOptions
                                                    {
                                                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder
                                                            .UnsafeRelaxedJsonEscaping,
                                                    })
                                                : string.Empty,
                                            Name = x.ToolUseName
                                        }
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