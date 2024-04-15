using AIDotNet.Abstractions;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace AIDotNet.OpenAI;

public sealed class OpenAIServiceTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        return await openAiService.Embeddings.CreateEmbedding(createEmbeddingModel, cancellationToken);
    }
}