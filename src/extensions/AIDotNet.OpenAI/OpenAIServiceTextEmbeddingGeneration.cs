using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Extensions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace AIDotNet.OpenAI;

public sealed class OpenAIServiceTextEmbeddingGeneration(IHttpClientFactory httpClientFactory)
    : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(OpenAIServiceOptions.ServiceName);

        var response = await client.PostJsonAsync(options?.Address.TrimEnd('/') + "/v1/embeddings",
            createEmbeddingModel, options!.Key);

        var result =
            await response.Content.ReadFromJsonAsync<EmbeddingCreateResponse>(cancellationToken: cancellationToken);

        return result;
    }
}