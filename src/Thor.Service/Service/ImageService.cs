using System.Text.RegularExpressions;
using SkiaSharp;

namespace Thor.Service.Service;

public class ImageService(IHttpClientFactory httpClientFactory):IScopeDependency
{
    private static readonly Regex DataUrlPattern = new(@"data:image/([^;]+);base64,(.*)", RegexOptions.Compiled);

    public async Task<bool> IsImageUrlAsync(string url)
    {
        var client = httpClientFactory.CreateClient(nameof(ImageService));

        var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (response.IsSuccessStatusCode)
        {
            var contentType = response.Content.Headers.ContentType;
            if (contentType != null) return contentType.MediaType?.StartsWith("image/") == true;
        }

        return false;
    }

    public async ValueTask<(int Width, int Height)> GetImageSizeFromUrlAsync(string url)
    {
        var isImage = await IsImageUrlAsync(url);
        if (!isImage) throw new Exception("URL is not an image.");

        var client = httpClientFactory.CreateClient(nameof(ImageService));
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var image = SKBitmap.Decode(stream);
            return (image.Width, image.Height);
        }

        throw new Exception("Failed to get image.");
    }

    public (string MimeType, string Data) GetImageFromUrl(string url)
    {
        var match = DataUrlPattern.Match(url);
        if (match.Success) return ("image/" + match.Groups[1].Value, match.Groups[2].Value);

        throw new Exception("URL is not a data URL.");
    }

    public (int Width, int Height) GetImageSizeFromBase64(string encoded)
    {
        var match = DataUrlPattern.Match(encoded);
        if (match.Success)
        {
            var bytes = Convert.FromBase64String(match.Groups[2].Value);
            using var ms = new MemoryStream(bytes);
            using var bitmap = SKBitmap.Decode(ms);
            return (bitmap.Width, bitmap.Height);
        }

        throw new Exception("String is not a valid base64 encoded image.");
    }

    public async ValueTask<(int Width, int Height)> GetImageSize(string image)
    {
        if (image.StartsWith("data:image/")) return GetImageSizeFromBase64(image);

        return await GetImageSizeFromUrlAsync(image);
    }
}