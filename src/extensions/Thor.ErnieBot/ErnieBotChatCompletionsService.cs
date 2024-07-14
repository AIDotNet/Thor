using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Chats.Consts;

namespace Thor.ErnieBot;

public class ErnieBotChatCompletionsService : IThorChatCompletionsService
{
    public async Task<ChatCompletionsResponse> ChatCompletionsAsync(Abstractions.Chats.Dtos.ThorChatCompletionsRequest chatCompletionCreate,
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

        var message = ThorChatMessage.CreateAssistantMessage(response.Result);
        return new ChatCompletionsResponse
        {
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Message =message,
                    Delta =message
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
        Abstractions.Chats.Dtos.ThorChatCompletionsRequest chatCompletionCreate, ChatPlatformOptions? options = null,
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

        await foreach (var item in client.ChatStreamAsync(chatRequest,
                           ErnieBotHelper.GetModelEndpoint(chatCompletionCreate.Model),
                           cancellationToken))
        {
            var message = ThorChatMessage.CreateAssistantMessage(item.Result);
            yield return new ChatCompletionsResponse()
            {
                Model = chatCompletionCreate.Model,
                Choices = new List<ChatChoiceResponse>()
                {
                    new()
                    {
                        Message = message,
                        Delta =message
                    }
                },
            };
        }
    }
}