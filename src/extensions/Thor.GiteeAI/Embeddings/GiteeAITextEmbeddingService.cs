using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.OpenAI.Embeddings;

public sealed class GiteeAITextEmbeddingService(IHttpClientFactory httpClientFactory)
    : IThorTextEmbeddingService
{
    private const string baseUrl = "https://ai.gitee.com/api/serverless/{0}/v1/embeddings";
    
    private static string GetBaseUrl(string model)
    {
        return string.Format(baseUrl, model);
    }

    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var url = GetBaseUrl(createEmbeddingModel.Model);
        
        var client = httpClientFactory.CreateClient(GiteeAIPlatformOptions.PlatformCode);

        var response = await client.PostJsonAsync(url,
            createEmbeddingModel, options!.ApiKey);

        var result =
            await response.Content.ReadFromJsonAsync<EmbeddingCreateResponse>(cancellationToken: cancellationToken);

        return result;
    }
}