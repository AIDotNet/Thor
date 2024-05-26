using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;
using OpenAI.ObjectModels.RequestModels;
using TencentCloud.Hunyuan.V20230901.Models;

namespace AIDotNet.Hunyuan;

public class HunyuanService : IApiChatCompletionService
{
    public async Task<ChatCompletionCreateResponse> CompleteChatAsync(ChatCompletionCreateRequest chatCompletionCreate,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.Key.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid  format, expected secretId|secretKey");

        // 解析key 从options中
        var secretId = keys[0];
        var secretKey = keys[1];

        var client = HunyuanHelper.CreateClient(secretId, secretKey, region: options.Other);

        var req = new ChatCompletionsRequest
        {
            EnableEnhancement = false,
            Stream = false,
            Model = chatCompletionCreate.Model,
            TopP = chatCompletionCreate.TopP,
            Temperature = chatCompletionCreate.Temperature,
            Messages = chatCompletionCreate.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray()
        };
        // 返回的resp是一个ChatCompletionsResponse的实例，与请求对象对应
        var resp = await client.ChatCompletions(req);

        return new ChatCompletionCreateResponse
        {
            Choices = resp.Choices.Select(x => new ChatChoiceResponse
            {
                Delta = new ChatMessage()
                {
                    Content = x.Message.Content,
                    Role = x.Message.Role,
                },
                Message = new ChatMessage()
                {
                    Content = x.Message.Content,
                    Role = x.Message.Role,
                }
            }).ToList(),
            Model = chatCompletionCreate.Model,
            Usage = new UsageResponse()
            {
                CompletionTokens = (int)(resp.Usage.CompletionTokens ?? 0),
                PromptTokens = (int)(resp.Usage.PromptTokens ?? 0),
                TotalTokens = (int)(resp.Usage.TotalTokens ?? 0)
            }
        };
    }

    public async IAsyncEnumerable<ChatCompletionCreateResponse> StreamChatAsync(
        ChatCompletionCreateRequest chatCompletionCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.Key.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid  format, expected secretId|secretKey");

        // 解析key 从options中
        var secretId = keys[0];
        var secretKey = keys[1];

        var client = HunyuanHelper.CreateClient(secretId, secretKey);

        var req = new ChatCompletionsRequest
        {
            EnableEnhancement = false,
            Stream = true,
            Model = chatCompletionCreate.Model,
            TopP = chatCompletionCreate.TopP,
            Temperature = chatCompletionCreate.Temperature,
            Messages = chatCompletionCreate.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray()
        };
        // 返回的resp是一个ChatCompletionsResponse的实例，与请求对象对应
        var resp = await client.ChatCompletions(req);


        foreach (var e in resp)
        {
            var v = JsonSerializer.Deserialize<ChatCompletionsResponse>(e.Data);
            var content = v?.Choices.FirstOrDefault()?.Message.Content;
            yield return new ChatCompletionCreateResponse
            {
                Choices = new List<ChatChoiceResponse>()
                {
                    new()
                    {
                        Delta = new ChatMessage()
                        {
                            Content = content,
                            Role = "assistant",
                        },
                        Message = new ChatMessage()
                        {
                            Content = content,
                            Role = "assistant",
                        }
                    }
                },
                Model = chatCompletionCreate.Model
            };
        }
    }
}