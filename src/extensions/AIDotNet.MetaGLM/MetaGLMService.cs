using System.Runtime.CompilerServices;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.MetaGLM.Models.RequestModels;
using OpenAI.ObjectModels.RequestModels;

namespace AIDotNet.MetaGLM;

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

        var result = await _openAiOptions.Client?.Chat.Completion(dto, options.Key, options.Address);

        return new ChatCompletionCreateResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = new ChatMessage("assistant", result.choices.FirstOrDefault()?.delta.content),
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

        await foreach (var result in _openAiOptions.Client?.Chat.Stream(dto, options.Key, options.Address))
        {
            yield return new ChatCompletionCreateResponse()
            {
                Choices =
                [
                    new()
                    {
                        Delta = new ChatMessage("assistant", result.choices.FirstOrDefault()?.delta.content),
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Model = input.Model
            };
        }
    }
}