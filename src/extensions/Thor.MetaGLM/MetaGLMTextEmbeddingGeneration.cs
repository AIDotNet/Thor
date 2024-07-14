using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.RequestModels;
using Thor.MetaGLM.Models.RequestModels;
using Thor.Abstractions.Embeddings;

namespace Thor.MetaGLM;

public class MetaGLMTextEmbeddingGeneration : IThorTextEmbeddingService
{
    private readonly MetaGLMPlatformOptions _openAiOptions;

    public MetaGLMTextEmbeddingGeneration()
    {
        _openAiOptions = new MetaGLMPlatformOptions
        {
            Client = new MetaGLMClientV4()
        };
    }

    public Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var embeddingRequestBase = new EmbeddingRequestBase();
        embeddingRequestBase.SetModel(createEmbeddingModel.Model);
        embeddingRequestBase.SetInput(createEmbeddingModel.Input);
        var response = _openAiOptions.Client!.Embeddings.Process(embeddingRequestBase, options.ApiKey);
        var embeddingCreateResponse = new EmbeddingCreateResponse
        {
            Model = createEmbeddingModel.Model,
            Data = response.data.Select(x => new EmbeddingResponse
            {
                Embedding = x.embedding.ToList(),
                Index = x.index,
            }).ToList()
        };
        return Task.FromResult(embeddingCreateResponse);
    }
}