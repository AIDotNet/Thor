using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Abstractions;

public interface IApiTextEmbeddingGeneration 
{
    Task<EmbeddingCreateResponse> EmbeddingAsync(
        EmbeddingCreateRequest createEmbeddingModel,
        ChatPlatformOptions? options = null,
        CancellationToken cancellationToken = default);
}