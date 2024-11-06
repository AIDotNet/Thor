using Sdcb.DashScope;
using Sdcb.DashScope.TextGeneration;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;
using ChatMessage = Sdcb.DashScope.TextGeneration.ChatMessage;

namespace Thor.Qiansail.Chats
{
    public sealed class QiansailChatCompletionsService : IThorChatCompletionsService
    {
        public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(
            ThorChatCompletionsRequest chatCompletionCreate, ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.ApiKey!);

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
                    Stop = chatCompletionCreate.Stop,
                    Tools = chatCompletionCreate.Tools?.Select(x => ChatTool.CreateFunction(
                        name: x.Function?.Name,
                        description: x.Function?.Description, 
                        parameters: x.Function.Parameters.Items.Properties.Select( 
                            y=> new FunctionParameter( 
                            y.Key,
                            y.Value.Type,
                            y.Value.Description?? string.Empty, y.Value.Required.Contains(y.Key)
                            )).ToArray())).ToArray()
                    .ToArray()
                },  cancellationToken);

            var toolsResult = new List<ThorToolCall>();
            if (result.Output.Choices[0].Message.ToolCalls != null && result.Output.Choices[0].Message.ToolCalls.Count() > 0)
            {
                foreach (var content in result.Output.Choices[0].Message.ToolCalls)
                {
                    toolsResult.Add(new ThorToolCall()
                    {
                        Function = new ThorChatMessageFunction()
                        {
                            Arguments = JsonSerializer.Serialize(content.Function?.Arguments),
                            Name = content.Function?.Name
                        }
                    });
                }
            }
            return new ThorChatCompletionsResponse()
            {
                Choices =
                [
                    new()
                    {
                        Delta =ThorChatMessage.CreateAssistantMessage(result.Output.Choices[0].Message.Content, toolCalls:toolsResult),
                        FinishReason = "stop",
                        Index = 0,
                    }
                ],
                Model = chatCompletionCreate.Model,
                Usage = new ThorUsageResponse()
                {
                    TotalTokens = result.Usage?.InputTokens + result.Usage?.OutputTokens ?? 0,
                    CompletionTokens = result.Usage?.OutputTokens,
                    PromptTokens = (result.Usage?.InputTokens) ?? 0,
                }
            };
        }

        public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
            ThorChatCompletionsRequest chatCompletionCreate, ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using DashScopeClient client = new(options!.ApiKey!);

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
                                   Stop = chatCompletionCreate.Stop,
                                   Tools = chatCompletionCreate.Tools?.Select(x => ChatTool.CreateFunction(
                        name: x.Function?.Name,
                        description: x.Function?.Description,
                        parameters: x.Function.Parameters.Items.Properties.Select(
                            y => new FunctionParameter(
                            y.Key,
                            y.Value.Type,
                            y.Value.Description ?? string.Empty, y.Value.Required.Contains(y.Key)
                            )).ToArray())).ToArray()
                    .ToArray(),
                                   IncrementalOutput = true
                               }, cancellationToken))
            {
                var toolsResult = new List<ThorToolCall>();
                if (result.Output.Choices[0].Message.ToolCalls != null && result.Output.Choices[0].Message.ToolCalls.Count() > 0)
                {
                    foreach (var content in result.Output.Choices[0].Message.ToolCalls)
                    {
                        toolsResult.Add(new ThorToolCall()
                        {
                            Function = new ThorChatMessageFunction()
                            {
                                Arguments = JsonSerializer.Serialize(content.Function?.Arguments),
                                Name = content.Function?.Name
                            }
                        });
                    }
                }
                var message = ThorChatMessage.CreateAssistantMessage(result.Output.Choices[0].Message.Content, toolCalls: toolsResult);
                yield return new ThorChatCompletionsResponse()
                {
                    Choices =
                    [
                        new()
                        {
                            Delta =message,
                            Message =message,
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