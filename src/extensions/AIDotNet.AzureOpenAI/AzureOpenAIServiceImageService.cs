using AIDotNet.Abstractions;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.RequestModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;
using Azure.AI.OpenAI;

namespace AIDotNet.AzureOpenAI;

public class AzureOpenAIServiceImageService : IApiImageService
{
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var client = AzureOpenAIHelper.CreateClient(options);

        var response = await client.GetImageGenerationsAsync(new ImageGenerationOptions(imageCreate.Prompt)
        {
            DeploymentName = imageCreate.Model,
            ImageCount = imageCreate.N,
            Size = imageCreate.Size,
            Style = new ImageGenerationStyle(imageCreate.Style),
            Quality = imageCreate.Quality
        }, cancellationToken).ConfigureAwait(false);

        var ret = new ImageCreateResponse()
        {
            Results = new List<ImageCreateResponse.ImageDataResult>()
        };

        foreach (var item in response.Value.Data)
        {
            ret.Results.Add(new ImageCreateResponse.ImageDataResult()
            {
                B64 = item.Base64Data,
                Url = item.Url.ToString(),
                RevisedPrompt = item.RevisedPrompt
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