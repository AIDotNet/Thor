using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.Responses;
using Thor.Abstractions.Responses.Dto;

namespace Thor.DeepSeek.Responses;

public class OpenAIResponsesService(ILogger<OpenAIResponsesService> logger) : IThorResponsesService
{
    public async Task<ResponsesDto> GetResponseAsync(ResponsesInput input, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        
        using var openai =
            Activity.Current?.Source.StartActivity("OpenAI 对话补全");

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
            options?.Address.TrimEnd('/') + "/v1/responses",
            input, options.ApiKey).ConfigureAwait(false);

        openai?.SetTag("Address", options?.Address.TrimEnd('/') + "/v1/responses");
        openai?.SetTag("Model", input.Model);
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
            logger.LogError("OpenAI对话异常 请求地址：{Address}, StatusCode: {StatusCode} Response: {Response}", options.Address,
                response.StatusCode, error);

            throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
        }

        var result =
            await response.Content.ReadFromJsonAsync<ResponsesDto>(
                cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }

    public IAsyncEnumerable<ResponsesDto> GetResponsesAsync(ResponsesInput input, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}