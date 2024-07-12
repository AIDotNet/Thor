using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Thor.Abstractions.Chats;

namespace Thor.ErnieBot;

public class ErnieBotChatCompletionsService : IChatCompletionsService
{
    public async Task<ChatCompletionsResponse> ChatCompletionsAsync(Abstractions.Chats.Dtos.ChatCompletionsRequest chatCompletionCreate,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.ApiKey!.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid format, expected ClientId|ClientSecret");

        var clientId = keys[0];
        var clientSecret = keys[1];

        var client = ErnieBotClientFactory.CreateClient(clientId, clientSecret);

        var chatRequest = new ChatRequest
        {
            Stream = false,
            Messages = chatCompletionCreate.Messages.Select(x => new Message()
            {
                Content = x.Content,
                Name = x.Name,
                Role = x.Role
            }).ToList()
        };


        var response = await client.ChatAsync(chatRequest, ErnieBotHelper.GetModelEndpoint(chatCompletionCreate.Model),
            cancellationToken);

        return new ChatCompletionsResponse
        {
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Message = new ChatMessage()
                    {
                        Role = "assistant",
                        Content = response.Result
                    },
                    Delta = new ChatMessage()
                    {
                        Role = "assistant",
                        Content = response.Result
                    }
                }
            },
            Model = chatCompletionCreate.Model,
            Usage = new UsageResponse
            {
                TotalTokens = response.Usage.TotalTokens,
                CompletionTokens = response.Usage.CompletionTokens,
                PromptTokens = response.Usage.PromptTokens
            }
        };
    }

    public async IAsyncEnumerable<ChatCompletionsResponse> StreamChatCompletionsAsync(
        Abstractions.Chats.Dtos.ChatCompletionsRequest chatCompletionCreate, ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.ApiKey!.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid format, expected ClientId|ClientSecret");

        var clientId = keys[0];
        var clientSecret = keys[1];

        var client = ErnieBotClientFactory.CreateClient(clientId, clientSecret);

        var chatRequest = new ChatRequest
        {
            Stream = false,
            Messages = chatCompletionCreate.Messages.Select(x => new Message()
            {
                Content = x.Content,
                Name = x.Name,
                Role = x.Role
            }).ToList()
        };

        await foreach (var item in  client.ChatStreamAsync(chatRequest,
                           ErnieBotHelper.GetModelEndpoint(chatCompletionCreate.Model),
                           cancellationToken))
        {
            yield return new ChatCompletionsResponse()
            {
                Model = chatCompletionCreate.Model,
                Choices = new List<ChatChoiceResponse>()
                {
                    new()
                    {
                        Message = new ChatMessage()
                        {
                            Role = "assistant",
                            Content = item.Result
                        },
                        Delta = new ChatMessage()
                        {
                            Role = "assistant",
                            Content = item.Result
                        }
                    }
                },
            };
        }
    }
}