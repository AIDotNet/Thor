using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Chats;
using Thor.Abstractions.Exceptions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.OpenAI.Chats;

public sealed class OpenAICompletionService : IThorCompletionsService
{
    public async Task<CompletionCreateResponse> CompletionAsync(CompletionCreateRequest createCompletionModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/chat/completions",
            createCompletionModel, options.ApiKey);

        // 如果限流则抛出限流异常
        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            throw new ThorRateLimitException();
        }

        var result = await response.Content.ReadFromJsonAsync<CompletionCreateResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }
}