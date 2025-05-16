using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;

namespace Thor.VolCenGine.Chats;

public sealed class VolCenGineChatCompletionsService(ILogger<VolCenGineChatCompletionsService> logger)
    : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest chatCompletionCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("VolCenGine 对话补全");

        if (string.IsNullOrWhiteSpace(options.Address))
        {
            options.Address = "https://ark.cn-beijing.volces.com/";
        }

        var headers = new Dictionary<string, string>
        {
            { "x-ark-moderation-scene", "skip-ark-moderation" }
        };

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
            options?.Address.TrimEnd('/') + "/api/v3/chat/completions",
            chatCompletionCreate, options.ApiKey, headers).ConfigureAwait(false);

        openai?.SetTag("Model", chatCompletionCreate.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new BusinessException("渠道未登录,请联系管理人员", "401");
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            logger.LogError("VolCenGine对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                error);

            throw new BusinessException("VolCenGine对话异常", response.StatusCode.ToString());
        }

        var result =
            await response.Content.ReadFromJsonAsync<ThorChatCompletionsResponse>(
                cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest chatCompletionCreate, ThorPlatformOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("VolCenGine 对话流式补全");

        if (string.IsNullOrWhiteSpace(options.Address))
        {
            options.Address = "https://ark.cn-beijing.volces.com/";
        }

        var response = await HttpClientFactory.GetHttpClient(options.Address).HttpRequestRaw(
            options?.Address.TrimEnd('/') + "/api/v3/chat/completions",
            chatCompletionCreate, options.ApiKey);

        openai?.SetTag("Model", chatCompletionCreate.Model);
        openai?.SetTag("Response", response.StatusCode.ToString());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        if (response.StatusCode == HttpStatusCode.PaymentRequired)
        {
            throw new PaymentRequiredException();
        }

        // 如果限流则抛出限流异常
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        // 大于等于400的状态码都认为是异常
        if (response.StatusCode >= HttpStatusCode.BadRequest)
        {
            logger.LogError("VolCenGine对话异常 , StatusCode: {StatusCode} ", response.StatusCode);

            throw new BusinessException("VolCenGine对话异常", response.StatusCode.ToString());
        }

        using var stream = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));

        using StreamReader reader = new(await response.Content.ReadAsStreamAsync(cancellationToken));
        string? line = string.Empty;
        var first = true;
        var isThink = false;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            line += Environment.NewLine;

            if (line.StartsWith('{'))
            {
                logger.LogInformation("VolCenGine对话异常 , StatusCode: {StatusCode} Response: {Response}",
                    response.StatusCode,
                    line);

                throw new BusinessException("VolCenGine对话异常", line);
            }

            if (line.StartsWith(OpenAIConstant.Data))
                line = line[OpenAIConstant.Done.Length..];

            line = line.Trim();

            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line == OpenAIConstant.Done)
            {
                break;
            }

            if (line.StartsWith(':'))
            {
                continue;
            }


            var result = JsonSerializer.Deserialize<ThorChatCompletionsResponse>(line,
                ThorJsonSerializer.DefaultOptions);

            var content = result?.Choices?.FirstOrDefault()?.Delta;
            
            if (first && content.Content == OpenAIConstant.ThinkStart)
            {
                isThink = true;
                continue;
            }

            if (isThink && content.Content.Contains(OpenAIConstant.ThinkEnd))
            {
                isThink = false;
                // 需要将content的内容转换到其他字段
                continue;
            }

            if (isThink)
            {
                // 需要将content的内容转换到其他字段
                foreach (var choice in result.Choices)
                {
                    choice.Delta.ReasoningContent = choice.Delta.Content;
                    choice.Delta.Content = string.Empty;
                }
            }

            first = false;

            yield return result;
        }
    }
}