using System.ClientModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Thor.Abstractions;
using Thor.Abstractions.Extensions;
using Thor.Abstractions.Images;
using Thor.Abstractions.ObjectModels.ObjectModels.RequestModels;
using Thor.Abstractions.ObjectModels.ObjectModels.ResponseModels.ImageResponseModel;

namespace Thor.OpenAI.Images;

public class GeminiImageService : IThorImageService
{
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("OpenAI 创建图片");

        if (string.IsNullOrEmpty(options?.Address))
        {
            options!.Address = "https://generativelanguage.googleapis.com/v1beta/openai/";
        }
        var client = HttpClientFactory.GetHttpClient(options.Address?.TrimEnd('/') + "/images/generations");

        openai?.SetTag("Prompt", imageCreate.Prompt);
        openai?.SetTag("Size", imageCreate.Size);
        openai?.SetTag("N", imageCreate.N.ToString());

        var stringContent = new StringContent(JsonSerializer.Serialize(imageCreate, ThorJsonSerializer.DefaultOptions),
            Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post,
            options.Address?.TrimEnd('/') + "/images/generations")
        {
            Content = stringContent,
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);

        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        var value = await response.Content.ReadFromJsonAsync<ImageCreateResponse>(ThorJsonSerializer.DefaultOptions,
            cancellationToken);

        if (value?.Error != null)
        {
            openai?.SetTag("Error", value.Error.Message);
        }

        return value!;
    }

    public async Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        using var openai =
            Activity.Current?.Source.StartActivity("OpenAI 创建图片编辑");

        if (string.IsNullOrEmpty(options?.Address))
        {
            options!.Address = "https://generativelanguage.googleapis.com/v1beta/openai/";
        }
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

        openai?.SetTag("Prompt", imageEditCreateRequest.Prompt);
        openai?.SetTag("Size", imageEditCreateRequest.Size);
        openai?.SetTag("N", imageEditCreateRequest.N.ToString());
        openai?.SetTag("Model", imageEditCreateRequest.Model);

        var client = HttpClientFactory.GetHttpClient(options.Address.TrimEnd('/') + "/images/edits");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, options.Address.TrimEnd('/') + "/images/edits");

        requestMessage.Content = multipartContent.HttpContent;

        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiKey);

        var response = await client.SendAsync(requestMessage, cancellationToken);

        var value = await response.Content.ReadFromJsonAsync<ImageCreateResponse>(cancellationToken: cancellationToken);

        if (value?.Error != null)
        {
            openai?.SetTag("Error", value.Error.Message);
        }

        return value!;
    }

    public async Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest,
        ThorPlatformOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var openai =
            Activity.Current?.Source.StartActivity("OpenAI 创建图片变体");

        if (string.IsNullOrEmpty(options?.Address))
        {
            options!.Address = "https://generativelanguage.googleapis.com/v1beta/openai/";
        }
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

        openai?.SetTag("Size", imageEditCreateRequest.Size);
        openai?.SetTag("N", imageEditCreateRequest.N.ToString());
        openai?.SetTag("Model", imageEditCreateRequest.Model);

        var value = await HttpClientFactory.GetHttpClient(options.Address).PostFileAndReadAsAsync<ImageCreateResponse>(
            options!.Address!.TrimEnd('/') + "/images/variations", multipartContent, cancellationToken);

        if (value?.Error != null)
        {
            openai?.SetTag("Error", value.Error.Message);
        }

        return value!;
    }
}