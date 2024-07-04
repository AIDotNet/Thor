using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using ERNIE_Bot.SDK;
using ERNIE_Bot.SDK.Models;

namespace Thor.ErnieBot;

public class ErnieBotTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.ApiKey!.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid format, expected ClientId|ClientSecret");

        var clientId = keys[0];
        var clientSecret = keys[1];

        var client = ErnieBotClientFactory.CreateClient(clientId, clientSecret);

        var response = await client.EmbeddingsAsync(new EmbeddingsRequest()
        {
            Input = createEmbeddingModel.InputCalculated?.ToList()
        }, new EmbeddingModelEndpoint(createEmbeddingModel.Model));

        return new EmbeddingCreateResponse()
        {
            Model = createEmbeddingModel.Model,
            Data = response.Data.Select(x => new EmbeddingResponse()
            {
                Embedding = x.Embedding,
                Index = x.Index
            }).ToList()
        };
    }
}