using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.ObjectModels.RequestModels;
using FunctionCall = AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels.FunctionCall;
using FunctionDefinition = Azure.AI.OpenAI.FunctionDefinition;

namespace AIDotNet.AzureOpenAI;

public class AzureOpenAiService : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = AzureOpenAIHelper.CreateClient(options);

        ChatCompletionsOptions chatCompletionsOptions = new()
        {
            DeploymentName = chatCompletionCreate.Model,
            MaxTokens = chatCompletionCreate.MaxTokens ?? 2048,
            Temperature = chatCompletionCreate.Temperature,
            FrequencyPenalty = chatCompletionCreate.FrequencyPenalty,
            Seed = chatCompletionCreate.Seed,
        };

        foreach (var message in chatCompletionCreate.Messages)
        {
            if (message.ContentCalculated is string)
            {
                if (message.Role.Equals("user"))
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(message.Content));
                }
                else if (message.Role.Equals("assistant"))
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(message.Content));
                }
                else
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage(message.Content));
                }
            }
            else
            {
                if (message.Role.Equals("user"))
                {
                    var messageContent = new List<ChatMessageContentItem>();
                    // 将内容转换为多个消息
                    foreach (var content in message.Contents)
                    {
                        if (content.Type == "text")
                            messageContent.Add(new ChatMessageTextContentItem(content.Text));
                        else if (content.Type == "image")
                        {
                            // 如果是base64图片
                            if (content.ImageUrl.Url.StartsWith("data:image"))
                                messageContent.Add(new ChatMessageImageContentItem(
                                    new BinaryData(Convert.FromBase64String(content.ImageUrl.Url.Split(",")[1])),
                                    "image/png"));
                            else
                                messageContent.Add(new ChatMessageImageContentItem(new Uri(content.ImageUrl.Url)));
                        }
                    }

                    chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(messageContent));
                }
                else if (message.Role.Equals("assistant"))
                {
                    foreach (var content in message.Contents)
                    {
                        chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(content.Text));
                    }
                }
                else
                {
                    foreach (var content in message.Contents)
                    {
                        chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage(content.Text));
                    }
                }
            }
        }


        if (chatCompletionCreate?.Tools is { Count: > 0 })
        {
            foreach (var tool in chatCompletionCreate.Tools)
            {
                chatCompletionsOptions.Functions.Add(new FunctionDefinition(tool.Function.Name)
                {
                    Description = tool.Function.Description,
                    Parameters = new BinaryData(tool.Function.Parameters)
                });
            }
        }

        var response = await client.GetChatCompletionsAsync(chatCompletionsOptions, cancellationToken)
            .ConfigureAwait(false);

        var createResponse = new ChatCompletionCreateResponse()
        {
            Usage = new UsageResponse(),
            Choices = new List<ChatChoiceResponse>()
        };

        foreach (var choice in response.Value.Choices)
        {
            var message = new ChatMessage("assistant", choice.Message.Content)
            {
                FunctionCall = new FunctionCall()
                {
                    Arguments = choice.Message?.FunctionCall?.Arguments,
                    Name = choice.Message?.FunctionCall?.Name
                },
                ToolCalls = choice?.Message?.ToolCalls?.Select(x =>
                {
                    if (x is ChatCompletionsFunctionToolCall toolCall)
                    {
                        return new ToolCall()
                        {
                            FunctionCall = new FunctionCall()
                            {
                                Arguments = toolCall.Arguments,
                                Name = toolCall.Name,
                            },
                        };
                    }

                    return new ToolCall();
                }).Where(x => !string.IsNullOrEmpty(x.Id)).ToList()
            };
            createResponse.Choices.Add(new ChatChoiceResponse()
            {
                Message = message,
                Delta = message,
            });
        }

        return createResponse;
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = AzureOpenAIHelper.CreateClient(options);

        ChatCompletionsOptions chatCompletionsOptions = new()
        {
            DeploymentName = chatCompletionCreate.Model,
            MaxTokens = chatCompletionCreate.MaxTokens,
            Temperature = chatCompletionCreate.Temperature,
            FrequencyPenalty = chatCompletionCreate.FrequencyPenalty,
            Seed = chatCompletionCreate.Seed,
        };

        foreach (var message in chatCompletionCreate.Messages)
        {
            if (message.ContentCalculated is string)
            {
                if (message.Role.Equals("user"))
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(message.Content));
                }
                else if (message.Role.Equals("assistant"))
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(message.Content));
                }
                else
                {
                    chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage(message.Content));
                }
            }
            else
            {
                if (message.Role.Equals("user"))
                {
                    var messageContent = new List<ChatMessageContentItem>();
                    // 将内容转换为多个消息
                    foreach (var content in message.Contents)
                    {
                        if (content.Type == "text")
                            messageContent.Add(new ChatMessageTextContentItem(content.Text));
                        else if (content.Type == "image")
                        {
                            // 如果是base64图片
                            if (content.ImageUrl.Url.StartsWith("data:image"))
                                messageContent.Add(new ChatMessageImageContentItem(
                                    new BinaryData(Convert.FromBase64String(content.ImageUrl.Url.Split(",")[1])),
                                    "image/png"));
                            else
                                messageContent.Add(new ChatMessageImageContentItem(new Uri(content.ImageUrl.Url)));
                        }
                    }

                    chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(messageContent));
                }
                else if (message.Role.Equals("assistant"))
                {
                    foreach (var content in message.Contents)
                    {
                        chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(content.Text));
                    }
                }
                else
                {
                    foreach (var content in message.Contents)
                    {
                        chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage(content.Text));
                    }
                }
            }
        }


        if (chatCompletionCreate?.Tools is { Count: > 0 })
        {
            foreach (var tool in chatCompletionCreate.Tools)
            {
                chatCompletionsOptions.Functions.Add(new FunctionDefinition(tool.Function.Name)
                {
                    Description = tool.Function.Description,
                    Parameters = new BinaryData(tool.Function.Parameters)
                });
            }
        }

        await foreach (var response in await client
                           .GetChatCompletionsStreamingAsync(chatCompletionsOptions, cancellationToken)
                           .ConfigureAwait(false))
        {
            var createResponse = new ChatCompletionCreateResponse()
            {
                Usage = new UsageResponse(),
                Choices = new List<ChatChoiceResponse>()
            };

            var delata = new ChatMessage("assistant", response.ContentUpdate);

            if (!string.IsNullOrEmpty(response.FunctionName))
            {
                delata.FunctionCall = new FunctionCall()
                {
                    Arguments = response.FunctionArgumentsUpdate,
                    Name = response.FunctionName,
                };
            }
            
            if(response.ToolCallUpdate is StreamingFunctionToolCallUpdate toolCallUpdate)
            {
                delata.ToolCalls = new List<ToolCall>();
                delata.ToolCalls.Add(new ToolCall()
                {
                    FunctionCall = new FunctionCall()
                    {
                        Arguments = toolCallUpdate.ArgumentsUpdate,
                        Name = toolCallUpdate.Name,
                    },
                });
            }

            createResponse.Choices.Add(new ChatChoiceResponse()
            {
                Delta = delata,
                FinishReason = response.FinishReason.ToString(),
                Index = response.ChoiceIndex,
                Message = delata,
            });

            yield return createResponse;
        }
    }
}