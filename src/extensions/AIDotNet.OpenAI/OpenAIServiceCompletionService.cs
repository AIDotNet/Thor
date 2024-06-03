using System.Net.Http.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Extensions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace AIDotNet.OpenAI;

public sealed class OpenAIServiceCompletionService(IHttpClientFactory httpClientFactory) : IApiCompletionService
{
    public async Task<CompletionCreateResponse> CompletionAsync(CompletionCreateRequest createCompletionModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(nameof(OpenAIServiceOptions.ServiceName));

        var response = await client.PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/chat/completions",
            createCompletionModel, options.Key);

        var result = await response.Content.ReadFromJsonAsync<CompletionCreateResponse>(
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return result;
    }
}