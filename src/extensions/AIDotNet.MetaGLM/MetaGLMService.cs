using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.MetaGLM.Models.RequestModels;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.MetaGLM;

public sealed class MetaGLMService : IChatCompletionService
{
    private readonly MetaGLMOptions _openAiOptions;

    public MetaGLMService()
    {
        _openAiOptions = new MetaGLMOptions
        {
            Client = new MetaGLMClientV4()
        };
    }

    public async Task<OpenAIResultDto> CompleteChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
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
        dto.SetTemperature(input.Temperature);
        dto.SetTopP(input.TopP);

        var result = await _openAiOptions.Client?.Chat.Completion(dto, options.Key, options.Address);

        return new OpenAIResultDto
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto
                {
                    Delta = new OpenAIMessageDto
                    {
                        Content = result.choices.FirstOrDefault()?.message.content,
                        Role = "assistant"
                    }
                }
            }
        };
    }

    public async IAsyncEnumerable<OpenAIResultDto> StreamChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var dto = new TextRequestBase();
        dto.SetRequestId(Guid.NewGuid().ToString());
        dto.SetMessages(input.Messages.Select(x => new MessageItem
        {
            content = x.Content ?? string.Empty,
            role = x.Role.ToString()
        }).Where(x => !string.IsNullOrEmpty(x.content)).ToArray());
        dto.SetModel(input.Model);
        dto.SetTemperature(input.Temperature);
        dto.SetTopP(input.TopP);

        await foreach (var item in _openAiOptions.Client?.Chat.Stream(dto, options.Key, options.Address))
        {
            yield return new OpenAIResultDto
            {
                Model = input.Model,
                Choices = new[]
                {
                    new OpenAIChoiceDto
                    {
                        Delta = new OpenAIMessageDto
                        {
                            Content = item.choices.FirstOrDefault()?.message.content,
                            Role = "assistant"
                        }
                    }
                }
            };
        }
    }

    public Task<OpenAIResultDto> FunctionCompleteChatAsync(
        OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}