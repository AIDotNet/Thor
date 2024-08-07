﻿using System.Runtime.InteropServices;
using Thor.Abstractions;
using Thor.Abstractions.Embeddings;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using Thor.SparkDesk.API;

namespace Thor.SparkDesk.Embeddings;

public sealed class SparkDeskTextEmbeddingService(IHttpClientFactory httpClientFactory) : IThorTextEmbeddingService
{
    private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(SparkDeskPlatformOptions.PlatformCode));

    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel, ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = SparkDeskFactory.GetSparkDeskEmbeddingClient(options?.ApiKey ?? "", HttpClient, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);
        var data = await client.GenerationAsync(new XFSparkDeskEmbeddingAPIRequest()
        {
            Domain = "para",
            Text = createEmbeddingModel.InputCalculated.First()
        }, cancellationToken);
        if (data.Header == null || data.Header.Code != 0 || data.Payload == null)
            throw new Exception(data.Header?.Message);

        var byteData = Convert.FromBase64String(data.Payload!.Feature.Text);
        float[] text = new float[byteData.Length / sizeof(float)];
        var handle = GCHandle.Alloc(byteData, GCHandleType.Pinned);
        try
        {
            nint ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, text, 0, text.Length);
        }
        finally
        {
            handle.Free();
        }

        return new EmbeddingCreateResponse()
        {
            Model = createEmbeddingModel.Model ?? "",
            Data = new List<EmbeddingResponse>()
            {
                new EmbeddingResponse()
                {
                    Embedding=text.Select(x=>(double)x).ToList()
                }
            }
        };
    }
}