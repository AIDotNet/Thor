using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Azure.AI.OpenAI;

namespace AIDotNet.AzureOpenAI;

public class AzureOpenAIServiceTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = AzureOpenAIHelper.CreateClient(options);

        var response = await client.GetEmbeddingsAsync(new EmbeddingsOptions(createEmbeddingModel.Model,
            createEmbeddingModel.InputCalculated), cancellationToken).ConfigureAwait(false);

        var embeddingCreateResponse = new EmbeddingCreateResponse()
        {
            Model = createEmbeddingModel.Model,
            Data =
            [
                ..response.Value.Data.Select(x => new EmbeddingResponse()
                {
                    Embedding = x.Embedding.ToArray().Select(x => (double)x).ToList(),
                    Index = x.Index
                })
            ]
        };

        return embeddingCreateResponse;
    }
}