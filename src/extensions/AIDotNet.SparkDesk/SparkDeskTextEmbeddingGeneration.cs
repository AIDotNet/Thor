using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.SparkDesk.API;
using OpenAI.ObjectModels.RequestModels;
using System.Runtime.InteropServices;
using System.Text;

namespace AIDotNet.SparkDesk;

public sealed class SparkDeskTextEmbeddingGeneration(IHttpClientFactory httpClientFactory) : IApiTextEmbeddingGeneration
{
    private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(SparkDeskOptions.ServiceName));

    public async Task<EmbeddingCreateResponse> EmbeddingAsync(EmbeddingCreateRequest createEmbeddingModel, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var client = SparkDeskHelper.GetSparkDeskEmbeddingClient(options?.Key ?? "", HttpClient, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);
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
            IntPtr ptr = handle.AddrOfPinnedObject();
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