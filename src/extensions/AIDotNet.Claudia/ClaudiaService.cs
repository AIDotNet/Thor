using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Claudia;
using OpenAI.ObjectModels.RequestModels;

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
            Temperature = input.Temperature,
        }, cancellationToken: cancellationToken);

        return new ChatCompletionCreateResponse()
        {
            Choices =
            [
                new()
                {
                    Delta = new ChatMessage("assistant", result.Content.FirstOrDefault()?.Text),
                    FinishReason = "stop",
                    Index = 0,
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
                            Delta = new ChatMessage("assistant", content.Delta.Text),
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