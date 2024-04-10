using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using Claudia;
using IChatCompletionService = AIDotNet.Abstractions.IChatCompletionService;

namespace AIDotNet.Claudia;

public sealed class ClaudiaService : IChatCompletionService
{
    public async Task<OpenAIResultDto> CompleteChatAsync(
        OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
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

        var message = await anthropic.Messages.CreateAsync(new MessageRequest
        {
            Model = input.Model,
            MaxTokens = input.MaxTokens,
            Messages = input.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray(),
            TopP = input.TopP,
            Temperature = input.Temperature,
        }, cancellationToken: cancellationToken);

        return new OpenAIResultDto
        {
            Model = input.Model,
            Choices = new[]
            {
                new OpenAIChoiceDto
                {
                    Delta = new OpenAIMessageDto
                    {
                        Content = message.Content.FirstOrDefault()?.Text ?? string.Empty,
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
        var anthropic = new Anthropic
        {
            ApiKey = options.Key,
        };
        if (!string.IsNullOrEmpty(options.Address))
        {
            anthropic.HttpClient.BaseAddress = new Uri(options.Address);
        }

        await foreach (var item in anthropic.Messages.CreateStreamAsync(new MessageRequest
                       {
                           Model = input.Model,
                           MaxTokens = input.MaxTokens,
                           Messages = input.Messages.Select(x => new Message
                           {
                               Content = x.Content,
                               Role = x.Role
                           }).ToArray(),
                           TopP = input.TopP,
                           Temperature = input.Temperature,
                       }, cancellationToken: cancellationToken))
        {
            if (item is ContentBlockDelta content)
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
                                Content = content.Delta.Text,
                                Role = "assistant"
                            }
                        }
                    }
                };
                Console.WriteLine();
            }
        }
    }

    public Task<OpenAIResultDto> FunctionCompleteChatAsync(
        OpenAIToolsFunctionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}