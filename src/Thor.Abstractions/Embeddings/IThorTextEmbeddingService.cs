using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions.Embeddings;

public interface IThorTextEmbeddingService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createEmbeddingModel"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<EmbeddingCreateResponse> EmbeddingAsync(
        EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}