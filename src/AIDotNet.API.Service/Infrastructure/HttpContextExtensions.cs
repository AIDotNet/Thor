using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using AIDotNet.Abstractions.Dto;

namespace AIDotNet.API.Service.Infrastructure;

public static class HttpContextExtensions
{
    public static async ValueTask WriteOpenAiResultAsync(this HttpContext context, string content, string model,
        string systemFingerprint, string id)
    {
        var openAiResult = new OpenAIResultDto()
        {
            Id = id,
            _object = "chat.completion.chunk",
            Created = DateTimeOffset.Now.ToUnixTimeSeconds(),
            Model = model,
            SystemFingerprint = systemFingerprint,
            Choices =
            [
                new OpenAIChoiceDto()
                {
                    Index = 0,
                    Delta = new()
                    {
                        Content = content,
                        Role = "assistant"
                    },
                    FinishReason = null
                }
            ]
        };

        await context.Response.WriteAsync("data: " + JsonSerializer.Serialize(openAiResult, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }) + "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    public static async ValueTask WriteOpenAiResultAsync(this HttpContext context, string content)
    {
        var openAiResult = new OpenAIResultDto()
        {
            Id = Guid.NewGuid().ToString("N"),
            _object = "chat.completion.chunk",
            Created = DateTimeOffset.Now.ToUnixTimeSeconds(),
            SystemFingerprint = Guid.NewGuid().ToString("N"),
            Choices =
            [
                new OpenAIChoiceDto()
                {
                    Index = 0,
                    Delta = new()
                    {
                        Content = content,
                        Role = "assistant"
                    },
                    FinishReason = null
                }
            ]
        };

        await context.Response.WriteAsync("data: " + JsonSerializer.Serialize(openAiResult, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }) + "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    public static async Task WriteEndAsync(this HttpContext context)
    {
        await context.Response.WriteAsync("data: [DONE]\n\n");
        await context.Response.Body.FlushAsync();
    }

    public static async ValueTask WriteEndAsync(this HttpContext context, string content)
    {
        await WriteOpenAiResultAsync(context, content);
        await WriteEndAsync(context);
    }
}