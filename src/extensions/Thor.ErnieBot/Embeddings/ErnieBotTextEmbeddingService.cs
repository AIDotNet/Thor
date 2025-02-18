using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.ErnieBot.Embeddings;

public class ErnieBotTextEmbeddingService : IThorTextEmbeddingService
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(options?.Address))
        {
            options.Address = "https://qianfan.baidubce.com/";
        }

        var response = await HttpClientFactory.GetHttpClient(options.Address).PostJsonAsync(
            options?.Address.TrimEnd('/') + "/v2/embeddings",
            createEmbeddingModel, options!.ApiKey);

        var result =
            await response.Content.ReadFromJsonAsync<EmbeddingCreateResponse>(cancellationToken: cancellationToken);

        return result;
    }
}