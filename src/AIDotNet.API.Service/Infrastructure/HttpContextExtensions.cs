using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using AIDotNet.Abstractions;
using AIDotNet.Abstractions.Dto;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.ResponseModels;
using AIDotNet.Abstractions.ObjectModels.ObjectModels.SharedModels;

namespace AIDotNet.API.Service.Infrastructure;

public static class HttpContextExtensions
{
    public static async ValueTask WriteResultAsync(this HttpContext context, object value)
    {
        var str = JsonSerializer.Serialize(value, AIDtoNetJsonSerializer.DefaultOptions);
        await context.Response.WriteAsync("data: " + str+ "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    public static async Task WriteEndAsync(this HttpContext context)
    {
        await context.Response.WriteAsync("data: [DONE]\n\n");
        await context.Response.Body.FlushAsync();
    }

    public static async ValueTask WriteStreamErrorAsync(this HttpContext context, string message)
    {
        var error = new ChatCompletionCreateResponse
        {
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Message = new()
                    {
                        ContentCalculated = message,
                        Role = "assistant",
                    },
                    Delta =
                    {
                        ContentCalculated = message,
                        Role = "assistant",
                    },
                    FinishReason = "error",
                    FinishDetails = new()
                    {
                        Type = "error",
                        Stop = "error",
                    },
                    Index = 0
                }
            }
        };
        await context.Response.WriteAsync(
            "data: " + JsonSerializer.Serialize(error, AIDtoNetJsonSerializer.DefaultOptions) + "\n\n", Encoding.UTF8);
        await context.WriteEndAsync();
    }

    public static async ValueTask WriteErrorAsync(this HttpContext context, string message)
    {
        var error = new ChatCompletionCreateResponse
        {
            Choices = new List<ChatChoiceResponse>()
            {
                new()
                {
                    Message = new()
                    {
                        ContentCalculated = message,
                        Role = "assistant",
                    },
                    Delta =
                    {
                        ContentCalculated = message,
                        Role = "assistant",
                    },
                    FinishReason = "error",
                    FinishDetails = new()
                    {
                        Type = "error",
                        Stop = "error",
                    },
                    Index = 0
                }
            }
        };
        await context.Response.WriteAsJsonAsync(error);
    }
}