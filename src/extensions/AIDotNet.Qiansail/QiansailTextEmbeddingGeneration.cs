using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Sdcb.DashScope;
using Sdcb.DashScope.TextEmbedding;

namespace AIDotNet.Qiansail;

public class QiansailTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using DashScopeClient client = new(options!.Key!);

        var result = await client.TextEmbedding.GetEmbeddings(new EmbeddingRequest
        {
            Model = createEmbeddingModel.Model,
            InputTexts = createEmbeddingModel.InputCalculated.ToList(),
        });

        return new EmbeddingCreateResponse()
        {
            Data = result.Output.Embeddings.Select(x => new EmbeddingResponse()
            {
                Embedding = x.Embedding.ToList(),
                Index = x.TextIndex
            }).ToList(),
            Model = createEmbeddingModel.Model,
            Usage = new UsageResponse()
            {
            }
        };
    }
}