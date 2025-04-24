using Thor.Abstractions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;
using Azure.AI.OpenAI;
using OpenAI.Images;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.Images;

namespace Thor.AzureOpenAI;

public class AzureOpenAIServiceImageService : IThorImageService
{
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ThorPlatformOptions? options = null,
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
            // User = imageCreate.User
            EndUserId = imageCreate.User
        }, cancellationToken);

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

    public async Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var url = AzureOpenAIFactory.GetEditImageAddress(options, imageEditCreateRequest.Model);
        
        var multipartContent = new MultipartFormDataContent();
        if (imageEditCreateRequest.User != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.User), "user");
        }

        if (imageEditCreateRequest.ResponseFormat != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.ResponseFormat), "response_format");
        }

        if (imageEditCreateRequest.Size != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Size), "size");
        }

        if (imageEditCreateRequest.N != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.N.ToString()!), "n");
        }

        if (imageEditCreateRequest.Model != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Model!), "model");
        }

        if (imageEditCreateRequest.Mask != null)
        {
            multipartContent.Add(new ByteArrayContent(imageEditCreateRequest.Mask), "mask",
                imageEditCreateRequest.MaskName);
        }

        multipartContent.Add(new StringContent(imageEditCreateRequest.Prompt), "prompt");
        multipartContent.Add(new ByteArrayContent(imageEditCreateRequest.Image), "image",
            imageEditCreateRequest.ImageName);

        return await HttpClientFactory.GetHttpClient(url).PostFileAndReadAsAsync<ImageCreateResponse>(
            url,
            multipartContent, cancellationToken);
    }

    public Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        throw new NotImplementedException();
    }
}