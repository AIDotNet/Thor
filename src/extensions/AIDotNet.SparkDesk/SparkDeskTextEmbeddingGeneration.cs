using AIDotNet.Abstractions;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace AIDotNet.SparkDesk;

public  sealed class SparkDeskTextEmbeddingGeneration :IApiTextEmbeddingGeneration
{
    public Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }
}