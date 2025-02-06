using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.SiliconFlow.Embeddings;

public sealed class SiliconFlowTextEmbeddingService(IHttpClientFactory httpClientFactory)
    : IThorTextEmbeddingService
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/embeddings",
            createEmbeddingModel, options!.ApiKey);

        var result =
            await response.Content.ReadFromJsonAsync<EmbeddingCreateResponse>(cancellationToken: cancellationToken);

        return result;
    }
}