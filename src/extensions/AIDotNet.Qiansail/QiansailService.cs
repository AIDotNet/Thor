using System.Runtime.CompilerServices;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.RequestModels;
using Sdcb.DashScope;
using Sdcb.DashScope.TextGeneration;
using ChatMessage = Sdcb.DashScope.TextGeneration.ChatMessage;

namespace AIDotNet.Qiansail
{
    public sealed class QiansailService : IApiChatCompletionService
    {
        public async Task<OpenAIResultDto> CompleteChatAsync(
            OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.Key!);

            var result = await client.TextGeneration.Chat(input.Model,
                input.Messages.Select(x => new ChatMessage(x.Role, x.Content)).ToArray(), new ChatParameters()
                {
                    MaxTokens = input.MaxTokens,
                    Temperature = (float?)input.Temperature,
                    TopP = (float?)input.TopP,
                }, cancellationToken);

            return new OpenAIResultDto()
            {
                Model = input.Model,
                Choices = new[]
                {
                    new OpenAIChoiceDto()
                    {
                        Delta = new OpenAIMessageDto()
                        {
                            Content = result.Output.Text,
                            Role = "assistant"
                        }
                    }
                }
            };
        }

        public async IAsyncEnumerable<OpenAIResultDto> StreamChatAsync(
            OpenAIChatCompletionInput<OpenAIChatCompletionRequestInput> input, ChatOptions? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.Key!);

            if (input.TopP >= 1)
            {
                input.TopP = 0.9;
            }
            else if (input.TopP <= 0)
            {
                input.TopP = 0.1;
            }

            await foreach (var item in client.TextGeneration.ChatStreamed(input.Model,
                               input.Messages.Select(x => new ChatMessage(x.Role, x.Content)).ToArray(),
                               new ChatParameters()
                               {
                                   MaxTokens = input.MaxTokens,
                                   Temperature = (float?)input.Temperature,
                                   TopP = (float?)input.TopP,
                               }, cancellationToken))
            {
                yield return new OpenAIResultDto()
                {
                    Model = input.Model,
                    Choices = new[]
                    {
                        new OpenAIChoiceDto()
                        {
                            Delta = new OpenAIMessageDto()
                            {
                                Content = item.Output.Text,
                                Role = "assistant"
                            }
                        }
                    }
                };
            }
        }

        public async Task<ChatCompletionCreateResponse> CompleteChatAsync(
            ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.Key!);

            if (chatCompletionCreate.TopP >= 1)
            {
                chatCompletionCreate.TopP = (float?)0.9;
            }
            else if (chatCompletionCreate.TopP <= 0)
            {
                chatCompletionCreate.TopP = (float?)0.1;
            }

            var result = await client.TextGeneration.Chat(chatCompletionCreate.Model,
                chatCompletionCreate.Messages.Select(x => new ChatMessage(x.Role, x.Content)).ToArray(),
                new ChatParameters()
                {
                    MaxTokens = chatCompletionCreate.MaxTokens,
                    Temperature = chatCompletionCreate.Temperature,
                    TopP = chatCompletionCreate.TopP,
                    ResultFormat = chatCompletionCreate.ResponseFormat?.Type,
                    Stop = chatCompletionCreate.Stop,
                }, cancellationToken);

            return new ChatCompletionCreateResponse()
            {
                Choices =
                [
                    new()
                    {
                        Delta = new OpenAI.ObjectModels.RequestModels.ChatMessage("assistant", result.Output.Text),
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Model = chatCompletionCreate.Model,
                Usage = new UsageResponse()
                {
                    TotalTokens = (result.Usage?.InputTokens + result.Usage?.OutputTokens) ?? 0,
                    CompletionTokens = result.Usage?.OutputTokens,
                    PromptTokens = (result.Usage?.InputTokens) ?? 0,
                }
            };
        }

        public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
            ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.Key!);

            if (chatCompletionCreate.TopP >= 1)
            {
                chatCompletionCreate.TopP = (float?)0.9;
            }
            else if (chatCompletionCreate.TopP <= 0)
            {
                chatCompletionCreate.TopP = (float?)0.1;
            }

            await foreach (var result in client.TextGeneration.ChatStreamed(chatCompletionCreate.Model,
                               chatCompletionCreate.Messages.Select(x => new ChatMessage(x.Role, x.Content)).ToArray(),
                               new ChatParameters()
                               {
                                   MaxTokens = chatCompletionCreate.MaxTokens,
                                   Temperature = chatCompletionCreate.Temperature,
                                   TopP = chatCompletionCreate.TopP,
                                   ResultFormat = chatCompletionCreate.ResponseFormat?.Type,
                                   Stop = chatCompletionCreate.Stop,
                               }, cancellationToken))
            {
                yield return new ChatCompletionCreateResponse()
                {
                    Choices =
                    [
                        new()
                        {
                            Delta = new OpenAI.ObjectModels.RequestModels.ChatMessage("assistant",
                                result.Output.Text),
                            FinishReason = "stop",
                            Index = 0,
                        }
                    ],
                    Model = chatCompletionCreate.Model
                };
            }
        }
    }
}