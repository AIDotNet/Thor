using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;
using AIDotNet.SparkDesk.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDotNet.SparkDesk
{
    public class SparkDeskImageService(IHttpClientFactory httpClientFactory) : IApiImageService
    {
        private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(SparkDeskOptions.ServiceName));

        public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var client = SparkDeskHelper.GetSparkDeskImageGenerationClient(options?.Key ?? "", HttpClient);
            var sizeInfo = imageCreate.Size?.Split("x") ?? ["512", "512"];
            var ret = new ImageCreateResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Results = new List<ImageCreateResponse.ImageDataResult>()
            };

            for (int i = 0; i < imageCreate.N; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var response = await client.GenerationAsync(new XFSparkDeskImageGenerationAPIRequest()
                {
                    Width = Convert.ToInt32(sizeInfo[0]),
                    Height = Convert.ToInt32(sizeInfo[1]),
                    Content = imageCreate.Prompt
                }, cancellationToken);

                if (response.Header?.Code != 0)
                    throw new Exception(response.Header?.Message);

                var imageData = response.Payload?.Choices.Text.FirstOrDefault()?.Content;
                if (imageData == null)
                    continue;

                ret.Results.Add(new ImageCreateResponse.ImageDataResult()
                {
                    B64 = imageData
                });
            }

            return ret;
        }

        public Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
