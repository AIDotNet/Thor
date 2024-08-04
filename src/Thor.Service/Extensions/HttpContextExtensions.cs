using Azure;
using System.Text;
using System.Text.Json;
using Thor.Abstractions.Chats.Dtos;
using Thor.Abstractions.Dtos;

namespace Thor.Service.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// 设置响应为 text/event-stream 相关的头
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static void SetEventStreamHeaders(this HttpContext context)
    {
        context.Response.ContentType = "text/event-stream;charset=utf-8;";
        context.Response.Headers.TryAdd("Cache-Control", "no-cache");
        context.Response.Headers.TryAdd("Connection", "keep-alive");
    }

    /// <summary>
    /// 往响应内容写入事件流数据,调用前需要先调用 <see cref="SetEventStreamHeaders"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async ValueTask WriteAsEventStreamDataAsync(this HttpContext context, object value)
    {
        var jsonData = JsonSerializer.Serialize(value, ThorJsonSerializer.DefaultOptions);
        await context.Response.WriteAsync($"data: {jsonData}\n\n", Encoding.UTF8);
        await context.Response.Body.FlushAsync();
    }

    /// <summary>
    /// 往响应内容写入事件流结束数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async ValueTask WriteAsEventStreamEndAsync(this HttpContext context)
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
                    Message = assistantMessage,
                    Delta = assistantMessage,
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

        await context.WriteAsEventStreamEndAsync();
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
                    Message = assistantMessage,
                    Delta = assistantMessage,
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

    /// <summary>
    /// 获取IP地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetIpAddress(this HttpContext context)
    {
        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString();
        }

        return ip;
    }

    /// <summary>
    /// 获取userAgent
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetUserAgent(this HttpContext context)
    {
        // 获取UserAgent，提取有用信息
        var userAgent = context.Request.Headers.UserAgent.FirstOrDefault();
        return userAgent ?? "未知";
    }
}