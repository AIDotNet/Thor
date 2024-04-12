using AIDotNet.Abstractions;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

namespace AIDotNet.OpenAI;

public class OpenAIServiceImageService : IApiImageService
{
    public Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        return openAiService.Image.CreateImage(imageCreate, cancellationToken);
    }

    public Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        return openAiService.Image.CreateImageEdit(imageEditCreateRequest, cancellationToken);
    }

    public Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = options.Key,
            BaseDomain = options.Address
        });

        return openAiService.Image.CreateImageVariation(imageEditCreateRequest, cancellationToken);
    }
}