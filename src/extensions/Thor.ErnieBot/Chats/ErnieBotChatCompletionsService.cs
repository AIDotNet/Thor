using ERNIE_Bot.SDK.Models;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.ObjectModels.ObjectModels.SharedModels;
using Thor.ErnieBot.Helpers;

namespace Thor.ErnieBot.Chats;

/// <summary>
/// 百度千帆对话补全服务
/// </summary>
public class ErnieBotChatCompletionsService : IThorChatCompletionsService
{

    /// <summary>
    /// 非流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(
        ThorChatCompletionsRequest request,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.ApiKey!.Split("|",StringSplitOptions.RemoveEmptyEntries);

        if (keys.Length != 2)
            throw new Exception("Key is invalid format, expected APIKey|SecretKey");

        var apiKey = keys[0];
        var secretKey = keys[1];

        var client = ErnieBotClientFactory.CreateClient(apiKey, secretKey);

        var chatRequest = new ChatRequest
        {
            Stream = false,
            Messages = request.Messages.Select(x => new Message()
            {
                Content = x.Content,
                Name = x.Name,
                Role = x.Role
            }).ToList()
        };


        var response = await client.ChatAsync(chatRequest, ErnieBotHelper.GetModelEndpoint(request.Model),
            cancellationToken);

        var message = ThorChatMessage.CreateAssistantMessage(response.Result);
        return new ThorChatCompletionsResponse
        {
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Message =message,
                    Delta =message
                }
            },
            Model = request.Model,
            Usage = new UsageResponse
            {
                TotalTokens = response.Usage.TotalTokens,
                CompletionTokens = response.Usage.CompletionTokens,
                PromptTokens = response.Usage.PromptTokens
            }
        };
    }

    /// <summary>
    /// 流式对话补全
    /// </summary>
    /// <param name="request">对话补全请求参数对象</param>
    /// <param name="options">平台参数对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest request, 
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.ApiKey!.Split("|", StringSplitOptions.RemoveEmptyEntries);

        if (keys.Length != 2)
            throw new Exception("Key is invalid format, expected APIKey|SecretKey");

        var apiKey = keys[0];
        var secretKey = keys[1];

        var client = ErnieBotClientFactory.CreateClient(apiKey, secretKey);

        var chatRequest = new ChatRequest
        {
            Stream = false,
            Messages = request.Messages.Select(x => new Message()
            {
                Content = x.Content,
                Name = x.Name,
                Role = x.Role
            }).ToList()
        };

        await foreach (var item in client.ChatStreamAsync(chatRequest,
                           ErnieBotHelper.GetModelEndpoint(request.Model),
                           cancellationToken))
        {
            var message = ThorChatMessage.CreateAssistantMessage(item.Result);
            yield return new ThorChatCompletionsResponse()
            {
                Model = request.Model,
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