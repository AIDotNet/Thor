using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;

namespace Thor.ErnieBot.Chats;

public class ErnieChatV2CompletionsService(ILogger<ErnieChatV2CompletionsService> logger) : IThorChatCompletionsService
{
    public async Task<ThorChatCompletionsResponse> ChatCompletionsAsync(ThorChatCompletionsRequest chatCompletionCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("百度千帆 对话补全");

        if (string.IsNullOrWhiteSpace(options?.Address))
        {
            options.Address = "https://qianfan.baidubce.com/";
        }

        if (string.IsNullOrWhiteSpace(options?.Other))
        {
            throw new BusinessException(
                "appId不能为空,请前往：https://console.bce.baidu.com/iam/#/iam/apikey/list 创建Key的时候进行绑定", "400");
        }

        var header = new Dictionary<string, string>(1) { { "appId", options.Other } };

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
                options?.Address.TrimEnd('/') + "/v2/chat/completions", chatCompletionCreate, options.ApiKey, header)
            .ConfigureAwait(false);

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
            logger.LogError("百度千帆对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode, error);

            throw new BusinessException("百度千帆对话异常", response.StatusCode.ToString());
        }

        var result =
            await response.Content.ReadFromJsonAsync<ThorChatCompletionsResponse>(
                cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }

    public async IAsyncEnumerable<ThorChatCompletionsResponse> StreamChatCompletionsAsync(
        ThorChatCompletionsRequest chatCompletionCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("OpenAI 对话流式补全");

        if (string.IsNullOrWhiteSpace(options?.Address))
        {
            options.Address = "https://qianfan.baidubce.com/";
        }

        if (string.IsNullOrWhiteSpace(options?.Other))
        {
            throw new BusinessException(
                "appId不能为空,请前往：https://console.bce.baidu.com/iam/#/iam/apikey/list 创建Key的时候进行绑定", "400");
        }

        var header = new Dictionary<string, string>(1) { { "appId", options.Other } };
        var response = await HttpClientFactory.GetHttpClient(options.Address).HttpRequestRaw(
            options?.Address.TrimEnd('/') + "/v2/chat/completions",
            chatCompletionCreate, options.ApiKey, header);

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
            logger.LogError("OpenAI对话异常 , StatusCode: {StatusCode} ", response.StatusCode);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
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
                logger.LogInformation("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    line);

                throw new BusinessException("OpenAI对话异常", line);
            }

            if (line.StartsWith(OpenAIConstant.Data))
                line = line[OpenAIConstant.Data.Length..];

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

            if (first && string.IsNullOrWhiteSpace(content?.Content) && string.IsNullOrEmpty(content?.ReasoningContent))
            {
                continue;
            }

            if (first && content.Content == OpenAIConstant.ThinkStart)
            {
                isThink = true;
                continue;
                // 需要将content的内容转换到其他字段
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