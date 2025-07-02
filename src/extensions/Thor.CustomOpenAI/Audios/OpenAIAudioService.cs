using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Thor.Abstractions;
using Thor.Abstractions.Audios;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.Abstractions.Realtime.Dto;

namespace Thor.CustomOpenAI.Audios
{
    public class OpenAIAudioService(ILogger<OpenAIAudioService> logger) : IThorAudioService
    {
        public async Task<AudioCreateTranscriptionResponse> TranscriptionsAsync(
            AudioCreateTranscriptionRequest request, ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using var openai =
                Activity.Current?.Source.StartActivity("OpenAI 语音转写");

            // 创建表单
            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(new MemoryStream(request.File)), "file", request.FileName);
            if (string.IsNullOrEmpty(request.Model))
            {
                throw new BusinessException("Model不能为空", "400");
            }

            form.Add(new StringContent(request.Model), "model");
            if (!string.IsNullOrEmpty(request.Language))
            {
                form.Add(new StringContent(request.Language), "language");
            }

            if (!string.IsNullOrEmpty(request.ResponseFormat))
            {
                form.Add(new StringContent(request.ResponseFormat), "response_format");
            }

            if (request.Temperature != null)
            {
                form.Add(new StringContent(request.Temperature.ToString()!), "temperature");
            }

            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, options?.Address.TrimEnd('/') + "/audio/transcriptions")
                {
                    Content = form
                };
            requestMessage.Headers.Add("Authorization", $"Bearer {options?.ApiKey}");

            var response = await HttpClientFactory.GetHttpClient(options.Address)
                .SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            openai?.SetTag("Model", request.Model);
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
                logger.LogError("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    error);

                throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
            }

            var result =
                await response.Content.ReadFromJsonAsync<AudioCreateTranscriptionResponse>(
                    cancellationToken: cancellationToken).ConfigureAwait(false);

            return result;
        }

        public async Task<AudioCreateTranscriptionResponse> TranslationsAsync(AudioCreateTranscriptionRequest request,
            ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using var openai =
                Activity.Current?.Source.StartActivity("OpenAI 语音转写");

            // 创建表单
            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(new MemoryStream(request.File)), "file", request.FileName);
            if (string.IsNullOrEmpty(request.Model))
            {
                throw new BusinessException("Model不能为空", "400");
            }

            form.Add(new StringContent(request.Model), "model");
            if (!string.IsNullOrEmpty(request.ResponseFormat))
            {
                form.Add(new StringContent(request.ResponseFormat), "response_format");
            }

            if (request.Temperature != null)
            {
                form.Add(new StringContent(request.Temperature.ToString()!), "temperature");
            }

            var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, options?.Address.TrimEnd('/') + "/audio/translations")
                {
                    Content = form
                };
            requestMessage.Headers.Add("Authorization", $"Bearer {options?.ApiKey}");

            var response = await HttpClientFactory.GetHttpClient(options.Address)
                .SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            openai?.SetTag("Model", request.Model);
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
                logger.LogError("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    error);

                throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
            }

            var result =
                await response.Content.ReadFromJsonAsync<AudioCreateTranscriptionResponse>(
                    cancellationToken: cancellationToken).ConfigureAwait(false);

            return result;
        }

        public async Task<(Stream, ThorUsageResponse? usage)> SpeechAsync(AudioCreateSpeechRequest request,
            ThorPlatformOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            using var openai =
                Activity.Current?.Source.StartActivity("OpenAI 文本转语音");

            var response = await HttpClientFactory.GetHttpClient(options.Address)
                .PostJsonAsync(options?.Address.TrimEnd('/') + "/audio/speech", request, options.ApiKey);

            openai?.SetTag("Model", request.Model);
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
                logger.LogError("OpenAI对话异常 , StatusCode: {StatusCode} Response: {Response}", response.StatusCode,
                    error);

                throw new BusinessException("OpenAI对话异常", response.StatusCode.ToString());
            }

            // 如果响应头有X-Usage
            if (response.Headers.TryGetValues("X-Usage", out var usageValues))
            {
                var usageJson = usageValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(usageJson))
                {
                    var usage = System.Text.Json.JsonSerializer.Deserialize<ThorUsageResponse>(usageJson);
                    return (await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false), usage);
                }
            }

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

            return (stream, null);
        }
    }
}