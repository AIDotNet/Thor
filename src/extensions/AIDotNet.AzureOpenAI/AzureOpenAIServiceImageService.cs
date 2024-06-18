using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;
using Azure.AI.OpenAI;
using OpenAI.Images;

namespace AIDotNet.AzureOpenAI;

public class AzureOpenAIServiceImageService : IApiImageService
{
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var createClient = AzureOpenAIFactory.CreateClient(options);

        var client = createClient.GetImageClient(imageCreate.Model);

        // 将size字符串拆分为宽度和高度
        var size = imageCreate.Size.Split('x');
        if (size.Length != 2)
        {
            throw new ArgumentException("Size must be in the format of 'width x height'");
        }


        var response = await client.GenerateImageAsync(imageCreate.Prompt, new ImageGenerationOptions()
        {
            Quality = imageCreate.Quality == "standard" ? GeneratedImageQuality.Standard : GeneratedImageQuality.High,
            Size = new GeneratedImageSize(Convert.ToInt32(size[0]), Convert.ToInt32(size[1])),
            Style = imageCreate.Style == "vivid" ? GeneratedImageStyle.Vivid : GeneratedImageStyle.Natural,
            ResponseFormat =
                imageCreate.ResponseFormat == "url" ? GeneratedImageFormat.Uri : GeneratedImageFormat.Bytes,
            User = imageCreate.User
        });

        var ret = new ImageCreateResponse()
        {
            Results = new List<ImageCreateResponse.ImageDataResult>()
        };

        if (response.Value.ImageUri != null)
        {
            ret.Results.Add(new ImageCreateResponse.ImageDataResult()
            {
                Url = response.Value.ImageUri.ToString()
            });
        }
        else
        {
            ret.Results.Add(new ImageCreateResponse.ImageDataResult()
            {
                B64 = Convert.ToBase64String(response.Value.ImageBytes.ToArray())
            });
        }

        return ret;
    }

    public Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }

    public Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }
}