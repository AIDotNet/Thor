using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;
using OpenAI.Chat;
using ChatMessage = OpenAI.Chat.ChatMessage;
using FunctionCall = AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels.FunctionCall;

namespace AIDotNet.AzureOpenAI;

public class AzureOpenAiService : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var createCreate = AzureOpenAIFactory.CreateClient(options);

        var client = createCreate.GetChatClient(chatCompletionCreate.Model);


        List<ChatMessage> chatCompletionsOptions = new();

        foreach (var message in chatCompletionCreate.Messages)
        {
            if (message.ContentCalculated is string)
            {
                switch (message.Role)
                {
                    case "user":
                        chatCompletionsOptions.Add(ChatMessage.CreateUserMessage(message.Content));
                        break;
                    case "assistant":
                        chatCompletionsOptions.Add(ChatMessage.CreateAssistantMessage(message.Content));
                        break;
                    case "system":
                        chatCompletionsOptions.Add(ChatMessage.CreateSystemMessage(message.Content));
                        break;
                    case "tool":
                        chatCompletionsOptions.Add(
                            ChatMessage.CreateToolChatMessage(message.ToolCallId, message.Content));
                        break;
                }
            }
            else
            {
                var messageContent = new List<ChatMessageContentPart>();
                switch (message.Role)
                {
                    case "user":
                        // 将内容转换为多个消息
                        foreach (var content in message.Contents)
                        {
                            if (content.Type == "text")
                                messageContent.Add(ChatMessageContentPart.CreateTextMessageContentPart(content.Text));
                            else if (content.Type == "image")
                            {
                                // 如果是base64图片
                                if (content.ImageUrl.Url.StartsWith("data:image"))
                                    messageContent.Add(ChatMessageContentPart.CreateImageMessageContentPart(
                                        new BinaryData(Convert.FromBase64String(content.ImageUrl.Url.Split(",")[1])),
                                        "image/png"));
                                else
                                    messageContent.Add(ChatMessageContentPart.CreateImageMessageContentPart(
                                        new Uri(content.ImageUrl.Url), content.ImageUrl.Detail));
                            }
                        }

                        chatCompletionsOptions.Add(ChatMessage.CreateUserMessage(messageContent));
                        break;
                }
            }
        }

        var chatCompletion = new ChatCompletionOptions();

        if (chatCompletionCreate?.Tools is { Count: > 0 })
        {
            foreach (var tool in chatCompletionCreate.Tools)
            {
                chatCompletion.Functions.Add(new(tool.Function?.Name)
                {
                    FunctionDescription = tool.Function?.Description,
                    FunctionParameters = BinaryData.FromObjectAsJson(tool.Function?.Parameters)
                });
            }
        }

        var response = await client.CompleteChatAsync(chatCompletionsOptions, options: chatCompletion,
            cancellationToken: cancellationToken);

        var createResponse = new ChatCompletionCreateResponse()
        {
            Usage = new UsageResponse(),
            Choices = new List<ChatChoiceResponse>()
        };

        foreach (var choice in response.Value.Content)
        {
            var message =
                new AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels.ChatMessage("assistant", choice.Text)
                {
                    FunctionCall = new FunctionCall()
                    {
                        Arguments = response.Value?.FunctionCall?.FunctionArguments,
                        Name = response.Value?.FunctionCall?.FunctionName,
                    },
                    ToolCalls = response.Value?.ToolCalls?.Select(x => new ToolCall()
                    {
                        FunctionCall = new FunctionCall()
                        {
                            Arguments = x.FunctionArguments,
                            Name = x.FunctionName,
                        },
                        Id = x.Id
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
        var createCreate = AzureOpenAIFactory.CreateClient(options);

        var client = createCreate.GetChatClient(chatCompletionCreate.Model);


        List<ChatMessage> chatCompletionsOptions = new();

        foreach (var message in chatCompletionCreate.Messages)
        {
            if (message.ContentCalculated is string)
            {
                switch (message.Role)
                {
                    case "user":
                        chatCompletionsOptions.Add(ChatMessage.CreateUserMessage(message.Content));
                        break;
                    case "assistant":
                        chatCompletionsOptions.Add(ChatMessage.CreateAssistantMessage(message.Content));
                        break;
                    case "system":
                        chatCompletionsOptions.Add(ChatMessage.CreateSystemMessage(message.Content));
                        break;
                    case "tool":
                        chatCompletionsOptions.Add(
                            ChatMessage.CreateToolChatMessage(message.ToolCallId, message.Content));
                        break;
                }
            }
            else
            {
                var messageContent = new List<ChatMessageContentPart>();
                switch (message.Role)
                {
                    case "user":
                        // 将内容转换为多个消息
                        foreach (var content in message.Contents)
                        {
                            if (content.Type == "text")
                                messageContent.Add(ChatMessageContentPart.CreateTextMessageContentPart(content.Text));
                            else if (content.Type == "image")
                            {
                                // 如果是base64图片
                                if (content.ImageUrl.Url.StartsWith("data:image"))
                                    messageContent.Add(ChatMessageContentPart.CreateImageMessageContentPart(
                                        new BinaryData(Convert.FromBase64String(content.ImageUrl.Url.Split(",")[1])),
                                        "image/png"));
                                else
                                    messageContent.Add(ChatMessageContentPart.CreateImageMessageContentPart(
                                        new Uri(content.ImageUrl.Url), content.ImageUrl.Detail));
                            }
                        }

                        chatCompletionsOptions.Add(ChatMessage.CreateUserMessage(messageContent));
                        break;
                }
            }
        }

        var chatCompletion = new ChatCompletionOptions();

        if (chatCompletionCreate?.Tools is { Count: > 0 })
        {
            foreach (var tool in chatCompletionCreate.Tools)
            {
                chatCompletion.Functions.Add(new(tool.Function?.Name)
                {
                    FunctionDescription = tool.Function?.Description,
                    FunctionParameters = BinaryData.FromObjectAsJson(tool.Function?.Parameters)
                });
            }
        }

        await foreach (var response in client
                           .CompleteChatStreamingAsync(chatCompletionsOptions, options: chatCompletion,
                               cancellationToken: cancellationToken)
                           .ConfigureAwait(false))
        {
            if (response.ContentUpdate is null)
                continue;

            foreach (var contentPart in response.ContentUpdate)
            {
                var message =
                    new AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels.ChatMessage("assistant",
                        contentPart.Text)
                    {
                        FunctionCall = new FunctionCall()
                        {
                            Arguments = response.FunctionCallUpdate.FunctionArgumentsUpdate,
                            Name = response.FunctionCallUpdate.FunctionName,
                        },
                        ToolCalls = response.ToolCallUpdates?.Select(x => new ToolCall()
                        {
                            FunctionCall = new FunctionCall()
                            {
                                Arguments = x.FunctionArgumentsUpdate,
                                Name = x.FunctionName,
                            },
                            Id = x.Id
                        }).Where(x => !string.IsNullOrEmpty(x.Id)).ToList()
                    };

                yield return new ChatCompletionCreateResponse()
                {
                    Usage = new UsageResponse(),
                    Choices = new List<ChatChoiceResponse>()
                    {
                        new()
                        {
                            Message = message,
                            Delta = message,
                        }
                    }
                };
            }
        }
    }
}