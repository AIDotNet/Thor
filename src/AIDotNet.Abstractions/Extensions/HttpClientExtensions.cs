using System.Text;
using System.Text.Json;

namespace AIDotNet.Abstractions.Extensions;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> HttpRequestRaw(this HttpClient httpClient, string url,
        object? postData,
        string token)
    {
        HttpRequestMessage req = new(HttpMethod.Post, url);

        if (postData != null)
        {
            if (postData is HttpContent data)
            {
                req.Content = data;
            }
            else
            {
                string jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            req.Headers.Add("Authorization", $"Bearer {token}");
        }


        var response = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);

        return response;
    }

    public static async Task<HttpResponseMessage> HttpRequestRaw(this HttpClient httpClient, HttpRequestMessage req,
        object? postData)
    {
        if (postData != null)
        {
            if (postData is HttpContent data)
            {
                req.Content = data;
            }
            else
            {
                string jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }

        var response = await httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);

        return response;
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string url, object? postData,
        string token)
    {
        HttpRequestMessage req = new(HttpMethod.Post, url);

        if (postData != null)
        {
            if (postData is HttpContent data)
            {
                req.Content = data;
            }
            else
            {
                string jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            req.Headers.Add("Authorization", $"Bearer {token}");
        }

        return httpClient.SendAsync(req);
    }
}