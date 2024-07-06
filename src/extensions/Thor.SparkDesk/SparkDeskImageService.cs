using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;
using Thor.SparkDesk.API;

namespace Thor.SparkDesk
{
    public class SparkDeskImageService(IHttpClientFactory httpClientFactory) : IApiImageService
    {
        private HttpClient HttpClient => httpClientFactory.CreateClient(nameof(SparkDeskPlatformOptions.PlatformCode));

        public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var client = SparkDeskFactory.GetSparkDeskImageGenerationClient(options?.ApiKey ?? "", HttpClient, string.IsNullOrWhiteSpace(options?.Address) ? null : options?.Address);
            var width = 512;
            var height = 512;
            try
            {
                var sizeInfo = imageCreate.Size?.Split("x") ?? ["512", "512"];
                width = Convert.ToInt32(sizeInfo[0]);
                height = Convert.ToInt32(sizeInfo[1]);
            }
            catch (Exception)
            {
                throw new Exception("create image size error");
            }
            var ret = new ImageCreateResponse()
            {
                Results = new List<ImageCreateResponse.ImageDataResult>()
            };

            for (int i = 0; i < imageCreate.N; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var response = await client.GenerationAsync(new XFSparkDeskImageGenerationAPIRequest()
                {
                    Width = width,
                    Height = height,
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
