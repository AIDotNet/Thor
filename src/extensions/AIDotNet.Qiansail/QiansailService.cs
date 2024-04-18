using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Sdcb.DashScope;
using Sdcb.DashScope.TextGeneration;
using ChatMessage = Sdcb.DashScope.TextGeneration.ChatMessage;

namespace AIDotNet.Qiansail
{
    public sealed class QiansailService : IApiChatCompletionService
    {
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