using System.Runtime.CompilerServices;
using Thor.Abstractions;
using Thor.Abstractions.Dto;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.RequestModels;
using Thor.MetaGLM.Models.RequestModels;
using Thor.MetaGLM.Models.RequestModels.FunctionModels;
using ChatCompletionCreateResponse = Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ChatCompletionCreateResponse;

namespace Thor.MetaGLM;

public sealed class MetaGLMService : IApiChatCompletionService
{
    private readonly MetaGLMOptions _openAiOptions;

    public MetaGLMService()
    {
        _openAiOptions = new MetaGLMOptions
        {
            Client = new MetaGLMClientV4()
        };
    }

    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest input,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(input.Messages.Select(x => new MessageItem
        {
            content = x.Content,
            role = x.Role.ToString()
        }).ToArray());
        dto.SetModel(input.Model);
        if (input.Temperature != null)
        {
            dto.SetTemperature(input.Temperature.Value);
        }

        if (input.TopP != null)
        {
            dto.SetTopP((double)input.TopP);
        }

        if (input.Tools != null)
        {
            foreach (var tool in input.Tools)
            {
                var functions = new FunctionTool();
                functions.type = tool.Type;
                if (!string.IsNullOrEmpty(tool.Function?.Name))
                    functions.SetName(tool.Function.Name);

                if (!string.IsNullOrEmpty(tool.Function?.Description))
                    functions.SetDescription(tool.Function.Description);

                var function = new FunctionParameters()
                {
                    required = tool.Function?.Parameters?.Required?.ToArray(),
                    type = tool.Function?.Parameters?.Type,
                };

                if (tool.Function?.Parameters?.Properties != null)
                {
                    foreach (var definition in tool.Function.Parameters.Properties)
                    {
                        function.properties.Add(definition.Key,
                            new FunctionParameterDescriptor(definition.Value.Type, definition.Value.Description));
                    }
                }

                functions.SetParameters(function);

                dto.tools.Add(functions);
            }
        }

        var result = await _openAiOptions.Client?.Chat.Completion(dto, options.Key, options.Address);

        var tools = new List<ToolCall>();
        foreach (var choiceItem in result.choices)
        {
            tools.AddRange(choiceItem.delta.tool_calls.Select(x => new ToolCall()
            {
                Id = x.id,
                Type = x.type,
                FunctionCall = new FunctionCall()
                {
                    Arguments = x.function.arguments,
                    Name = x.function.name,
                }
            }));
        }

        return new ChatCompletionCreateResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = new ChatMessage("assistant", result.choices.FirstOrDefault()?.delta.content ?? string.Empty,
                        null, tools),
                    Message = new ChatMessage("assistant",
                        result.choices.FirstOrDefault()?.delta.content ?? string.Empty, null, tools),
                    FinishReason = "stop",
                    Index = 0,
                }
            ],
            Model = input.Model
        };
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(ChatCompletionCreateRequest input,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(input.Messages.Select(x => new MessageItem
        {
            content = x.Content!,
            role = x.Role.ToString()
        }).ToArray());
        dto.SetModel(input.Model!);
        if (input.Temperature != null)
        {
            dto.SetTemperature(input.Temperature.Value);
        }

        if (input.TopP != null)
        {
            dto.SetTopP((double)input.TopP);
        }

        if (input.Tools != null)
        {
            foreach (var tool in input.Tools)
            {
                var functions = new FunctionTool();
                functions.type = tool.Type;
                if (!string.IsNullOrEmpty(tool.Function?.Name))
                    functions.SetName(tool.Function.Name);

                if (!string.IsNullOrEmpty(tool.Function?.Description))
                    functions.SetDescription(tool.Function.Description);

                var function = new FunctionParameters()
                {
                    required = tool.Function?.Parameters?.Required?.ToArray(),
                    type = tool.Function?.Parameters?.Type,
                };

                if (tool.Function?.Parameters?.Properties != null)
                {
                    foreach (var definition in tool.Function.Parameters.Properties)
                    {
                        function.properties.Add(definition.Key,
                            new FunctionParameterDescriptor(definition.Value.Type, definition.Value.Description));
                    }
                }

                functions.SetParameters(function);

                dto.tools.Add(functions);
            }
        }

        await foreach (var result in _openAiOptions.Client?.Chat.Stream(dto, options.Key, options.Address))
        {
            var tools = new List<ToolCall>();
            foreach (var choiceItem in result.choices)
            {
                tools.AddRange(choiceItem.delta.tool_calls.Select(x => new ToolCall()
                {
                    Id = x.id,
                    Type = x.type,
                    FunctionCall = new FunctionCall()
                    {
                        Arguments = x.function.arguments,
                        Name = x.function.name,
                    }
                }));
            }

            yield return new ChatCompletionCreateResponse()
            {
                Choices =
                [
                    new()
                    {
                        Delta = new ChatMessage("assistant",
                            result.choices.FirstOrDefault()?.delta.content ?? string.Empty, null, tools),
                        Message = new ChatMessage("assistant",
                            result.choices.FirstOrDefault()?.delta.content ?? string.Empty, null, tools),
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Model = input.Model
            };
        }
    }
}