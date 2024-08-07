﻿using System.Text.Json;
using TencentCloud.Hunyuan.V20230901.Models;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;

namespace Thor.Hunyuan.Chats;

/// <summary>
///腾讯混元对话补全服务
/// </summary>
public class HunyuanChatCompletionsService : IThorChatCompletionsService
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
        var keys = options!.ApiKey.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid  format, expected secretId|secretKey");

        // 解析key 从options中
        var secretId = keys[0];
        var secretKey = keys[1];

        var client = HunyuanFactory.CreateClient(secretId, secretKey, region: options.Other);

        var req = new ChatCompletionsRequest
        {
            EnableEnhancement = false,
            Stream = false,
            Model = request.Model,
            TopP = request.TopP,
            Temperature = request.Temperature,
            Messages = request.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray()
        };
        // 返回的resp是一个ChatCompletionsResponse的实例，与请求对象对应
        var resp = await client.ChatCompletions(req);

        return new ThorChatCompletionsResponse
        {
            Choices = resp.Choices.Select(x =>
            {
                var message = ThorChatMessage.CreateAssistantMessage(x.Message.Content);
                return new ThorChatChoiceResponse
                {
                    Delta = message,
                    Message = message
                };
            }).ToList(),
            Model = request.Model,
            Usage = new ThorUsageResponse()
            {
                CompletionTokens = (int)(resp.Usage.CompletionTokens ?? 0),
                PromptTokens = (int)(resp.Usage.PromptTokens ?? 0),
                TotalTokens = (int)(resp.Usage.TotalTokens ?? 0)
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
        var keys = options!.ApiKey.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid  format, expected secretId|secretKey");

        // 解析key 从options中
        var secretId = keys[0];
        var secretKey = keys[1];

        var client = HunyuanFactory.CreateClient(secretId, secretKey);

        var req = new ChatCompletionsRequest
        {
            EnableEnhancement = false,
            Stream = true,
            Model = request.Model,
            TopP = request.TopP,
            Temperature = request.Temperature,
            Messages = request.Messages.Select(x => new Message
            {
                Content = x.Content,
                Role = x.Role
            }).ToArray()
        };
        // 返回的resp是一个ChatCompletionsResponse的实例，与请求对象对应
        var resp = await client.ChatCompletions(req);


        foreach (var e in resp)
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                continue;
            }

            var v = JsonSerializer.Deserialize<HunyuanResultDto>(e.Data);
            var content = v?.Choices.FirstOrDefault()?.Delta.Content;

            var message = ThorChatMessage.CreateAssistantMessage(content);

            yield return new ThorChatCompletionsResponse
            {
                Choices = new List<ThorChatChoiceResponse>()
                {
                    new()
                    {
                        Delta = message,
                        Message =message
                    }
                },
                Model = request.Model
            };
        }
    }

    public class HunyuanResultDto
    {
        public string Note { get; set; }
        public HunyuanResultChoices[] Choices { get; set; }
        public int Created { get; set; }
        public string Id { get; set; }
        public HunyuanResultUsage Usage { get; set; }
    }

    public class HunyuanResultChoices
    {
        public HunyuanResultDelta Delta { get; set; }
        public string FinishReason { get; set; }
    }

    public class HunyuanResultDelta
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class HunyuanResultUsage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}