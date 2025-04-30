using System.ClientModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Thor.Abstractions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.Images;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;

namespace Thor.OpenAI.Images;

public class OpenAIImageService : IThorImageService
{
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.GetHttpClient(options.Address?.TrimEnd('/') + "/v1/images/generations");
        var response = await client.PostJsonAsync(
            options.Address?.TrimEnd('/') + "/v1/images/generations",
            imageCreate, options.ApiKey);

        var result =
            await response.Content.ReadFromJsonAsync<ImageCreateResponse>(cancellationToken: cancellationToken);

        return result;
    }

    public async Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultiPartFormDataBinaryContent();
        if (imageEditCreateRequest.User != null)
        {
            multipartContent.Add(imageEditCreateRequest.User, "user");
        }

        if (imageEditCreateRequest.ResponseFormat != null)
        {
            multipartContent.Add(imageEditCreateRequest.ResponseFormat, "response_format");
        }

        if (imageEditCreateRequest.Size != null)
        {
            multipartContent.Add(imageEditCreateRequest.Size, "size");
        }

        if (imageEditCreateRequest.N != null)
        {
            multipartContent.Add(imageEditCreateRequest.N.ToString(), "n");
        }

        if (imageEditCreateRequest.Model != null)
        {
            multipartContent.Add(imageEditCreateRequest.Model!, "model");
        }

        if (imageEditCreateRequest.Mask != null)
        {
            multipartContent.Add(imageEditCreateRequest.Mask, "mask",
                imageEditCreateRequest.MaskName);
        }

        multipartContent.Add(imageEditCreateRequest.Prompt, "prompt");
        multipartContent.Add(imageEditCreateRequest.Image, "image",
            imageEditCreateRequest.ImageName);

        var client = HttpClientFactory.GetHttpClient(options.Address);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, options.Address.TrimEnd('/') + "/v1/images/edits");

        requestMessage.Content = multipartContent.HttpContent;

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);

        var response = await client.SendAsync(requestMessage, cancellationToken);

        return await response.Content.ReadFromJsonAsync<ImageCreateResponse>();
    }

    public async Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
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

        return await HttpClientFactory.GetHttpClient(options.Address).PostFileAndReadAsAsync<ImageCreateResponse>(
            options!.Address!.TrimEnd('/') + "/v1/images/variations", multipartContent, cancellationToken);
    }
}