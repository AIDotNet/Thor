using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;

namespace Thor.OpenAI;

public class OpenAIServiceImageService(IHttpClientFactory httpClientFactory) : IApiImageService
{
    private HttpClient HttpClient => httpClientFactory.CreateClient(OpenAIServiceOptions.ServiceName);

    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await HttpClient.PostJsonAsync(
            options.Address?.TrimEnd('/') + "/v1/images/generations",
            imageCreate, options.ApiKey);
        
        var result = await response.Content.ReadFromJsonAsync<ImageCreateResponse>(cancellationToken: cancellationToken);
        
        return result;
    }

    public async Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default(CancellationToken))
    {
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

        return await HttpClient.PostFileAndReadAsAsync<ImageCreateResponse>(
            options.Address.TrimEnd('/') + "/v1/images/edits",
            multipartContent, cancellationToken);
    }

    public async Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
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

        multipartContent.Add(new ByteArrayContent(imageEditCreateRequest.Image), "image",
            imageEditCreateRequest.ImageName);

        return await HttpClient.PostFileAndReadAsAsync<ImageCreateResponse>(
            options!.Address!.TrimEnd('/') + "/v1//images/variations", multipartContent, cancellationToken);
    }
}