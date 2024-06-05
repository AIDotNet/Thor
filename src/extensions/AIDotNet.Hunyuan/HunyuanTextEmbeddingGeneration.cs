using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using TencentCloud.Hunyuan.V20230901.Models;

namespace AIDotNet.Hunyuan;

public class HunyuanTextEmbeddingGeneration : IApiTextEmbeddingGeneration
{
    public Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var keys = options!.Key.Split("|");

        if (keys.Length != 2)
            throw new Exception("Key is invalid  format, expected secretId|secretKey");

        // 解析key 从options中
        var secretId = keys[0];
        var secretKey = keys[1];

        var client = HunyuanFactory.CreateClient(secretId, secretKey);

        var req = new GetEmbeddingRequest()
        {
            Input = createEmbeddingModel.Input,
        };
        var resp = client.GetEmbeddingSync(req);

        return Task.FromResult(new EmbeddingCreateResponse()
        {
            Model = createEmbeddingModel.Model,
            Data = resp.Data.Select(x => new EmbeddingResponse()
            {
                Index = (int)(x.Index ?? 0),
                Embedding = x.Embedding.Select(x => (double)x).ToList()
            }).ToList(),
        });
    }
}