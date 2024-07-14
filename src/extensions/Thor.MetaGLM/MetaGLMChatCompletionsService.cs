using System.Runtime.CompilerServices;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.MetaGLM.Models.RequestModels;
using Thor.MetaGLM.Models.RequestModels.FunctionModels;
using ChatCompletionsResponse = Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ChatCompletionsResponse;

namespace Thor.MetaGLM;

public sealed class MetaGLMChatCompletionsService : IThorChatCompletionsService
{
    private readonly MetaGLMPlatformOptions _openAiOptions;

    public MetaGLMChatCompletionsService()
    {
        _openAiOptions = new MetaGLMPlatformOptions
        {
            Client = new MetaGLMClientV4()
        };
    }

    public async Task<ChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest input,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var dto = new TextRequestBase();
        dto.request_id = Guid.NewGuid().ToString();

        dto.messages.AddRange(input.Messages.Select(x => new MessageItem
        {
            content = x.Content,
            role = x.Role.ToString()
        }).ToList());

        dto.model = input.Model;

        if (input.Temperature.HasValue)
        {
            dto.temperature = input.Temperature.Value;
        }

        if (input.TopP.HasValue)
        {
            dto.top_p = input.TopP.Value;
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

        var result = await _openAiOptions.Client?.Chat.Completion(dto, options.ApiKey, options.Address);

        if (result.error != null)
        {
            throw new Exception($"code:{result.error["code"]},message:{result.error["message"]}");
        }

        var tools = new List<ThorToolCall>();
        foreach (var choiceItem in result.choices)
        {
            if (choiceItem.message.tool_calls == null)
            {
                continue;
            }

            tools.AddRange(choiceItem.message?.tool_calls?.Select(x => new ThorToolCall()
            {
                Id = x.id,
                Type = x.type,
                Function = new ThorChatMessageFunction()
                {
                    Arguments = x.function.arguments,
                    Name = x.function.name,
                }
            }));
        }

        return new ChatCompletionsResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = ThorChatMessage.CreateAssistantMessage(result.choices.FirstOrDefault()?.message.content ?? string.Empty,
                        null, tools),
                    Message = ThorChatMessage.CreateAssistantMessage(result.choices.FirstOrDefault()?.message.content ?? string.Empty, null, tools),
                    FinishReason = "stop",
                    Index = 0,
                }
            ],
            Model = input.Model
        };
    }

    public async IAsyncEnumerable<ChatCompletionsResponse> StreamChatCompletionsAsync(ThorChatCompletionsRequest input,
        ChatPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var dto = new TextRequestBase();
        dto.request_id = Guid.NewGuid().ToString();
        dto.messages.AddRange(input.Messages.Select(x => new MessageItem
        {
            content = x.Content!,
            role = x.Role.ToString()
        }).ToList());

        dto.model = input.Model!;

        if (input.Temperature.HasValue)
        {
            dto.temperature = input.Temperature.Value;
        }

        if (input.TopP.HasValue)
        {
            dto.top_p = input.TopP.Value;
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

        await foreach (var result in _openAiOptions.Client?.Chat.Stream(dto, options.ApiKey, options.Address))
        {
            var tools = new List<ThorToolCall>();
            foreach (var choiceItem in result.choices)
            {
                if (choiceItem.delta.tool_calls is null)
                {
                    continue;
                }

                tools.AddRange(choiceItem.delta.tool_calls.Select(x => new ThorToolCall()
                {
                    Id = x.id,
                    Type = x.type,
                    Function = new ThorChatMessageFunction()
                    {
                        Arguments = x.function.arguments,
                        Name = x.function.name,
                    }
                }));
            }

            var message = ThorChatMessage.CreateAssistantMessage(result.choices.FirstOrDefault()?.delta.content ?? string.Empty, toolCalls: tools);
            yield return new ChatCompletionsResponse()
            {
                Choices =
                [
                    new()
                    {
                        Delta = message,
                        Message =message,
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Model = input.Model
            };
        }
    }
}