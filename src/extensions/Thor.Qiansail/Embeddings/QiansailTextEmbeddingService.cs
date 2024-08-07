﻿using Sdcb.DashScope;
using Sdcb.DashScope.TextEmbedding;
using Thor.Abstractions;
using Thor.Abstractions.Dtos;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;

namespace Thor.Qiansail.Embeddings;

public class QiansailTextEmbeddingService : IThorTextEmbeddingService
{
    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using DashScopeClient client = new(options!.ApiKey!);

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
            Usage = new ThorUsageResponse()
            {
            }
        };
    }
}