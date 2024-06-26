﻿using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Azure.AI.OpenAI;

namespace Thor.AzureOpenAI;

public class AzureOpenAIServiceTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var createClient = AzureOpenAIFactory.CreateClient(options);

        var client = createClient.GetEmbeddingClient(createEmbeddingModel.Model);
        if (createEmbeddingModel.InputCalculated is string)
        {
        }

        var response = await client.GenerateEmbeddingsAsync(createEmbeddingModel.InputCalculated?.ToArray(),
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var embeddingCreateResponse = new EmbeddingCreateResponse()
        {
            Model = createEmbeddingModel.Model,
            Data =
            [
                ..response.Value.Select(x => new EmbeddingResponse()
                {
                    Embedding = x.Vector.ToArray().Select(x => (double)x).ToList(),
                    Index = x.Index
                })
            ]
        };

        return embeddingCreateResponse;
    }
}