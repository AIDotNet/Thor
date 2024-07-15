using System.Text;
using System.Text.Json;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;

namespace Thor.Service.Infrastructure;

public static class HttpContextExtensions
{
    public static async ValueTask WriteResultAsync(this HttpContext context, object value)
    {
        var str = JsonSerializer.Serialize(value, ThorJsonSerializer.DefaultOptions);
        await context.Response.WriteAsync("data: " + str + "\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    public static async Task WriteEndAsync(this HttpContext context)
    {
        await context.Response.WriteAsync("data: [DONE]\n\n");
        await context.Response.Body.FlushAsync();
    }

    public static async ValueTask WriteStreamErrorAsync(this HttpContext context, string message, string code)
    {
        var error = new ThorChatCompletionsResponse
        {
            Error = new ThorError()
            {
                MessageObject = message,
                Type = "error",
                Code = code
            }
        };

        context.Response.Headers.ContentType = "text/event-stream";

        await context.Response.WriteAsync(
            "data: " + JsonSerializer.Serialize(error, ThorJsonSerializer.DefaultOptions) + "\n\n", Encoding.UTF8);
    }

    public static async ValueTask WriteStreamErrorAsync(this HttpContext context, string message)
    {
        var assistantMessage = ThorChatMessage.CreateAssistantMessage(message);
        var error = new ThorChatCompletionsResponse
        {
            Choices = new List<ThorChatChoiceResponse>()
            {
                new()
                {
                    Message =assistantMessage,
                    Delta =assistantMessage,
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

        context.Response.Headers.ContentType = "text/event-stream";

        await context.Response.WriteAsync(
            "data: " + JsonSerializer.Serialize(error, ThorJsonSerializer.DefaultOptions) + "\n\n", Encoding.UTF8);

        await context.WriteEndAsync();
    }

    public static async ValueTask WriteErrorAsync(this HttpContext context, string message)
    {
        var assistantMessage = ThorChatMessage.CreateAssistantMessage(message);
        var error = new ThorChatCompletionsResponse
        {
            Choices = new List<ThorChatChoiceResponse>()
            {
                new()
                {
                    Message =assistantMessage,
                    Delta =assistantMessage,
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


    public static async ValueTask WriteErrorAsync(this HttpContext context, string message, string code)
    {
        var error = new ThorChatCompletionsResponse
        {
            Error = new ThorError()
            {
                MessageObject = message,
                Code = code
            }
        };
        await context.Response.WriteAsJsonAsync(error);
    }
}