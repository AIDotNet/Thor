using System.Text;
using System.Text.Json;
using Thor.Core.DataAccess;
using Thor.Service.Service;
using Thor.Domain.Chats;

namespace Thor.Service.Infrastructure.Middlewares;

/// <summary>
/// 详细请求响应日志中间件
/// </summary>
public class DetailedRequestResponseLogMiddleware : IMiddleware
{
    private readonly ILogger<DetailedRequestResponseLogMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DetailedRequestResponseLogMiddleware(
        ILogger<DetailedRequestResponseLogMiddleware> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // 检查是否启用详细日志
            var enableDetailedLog = SettingService.GetBoolSetting(SettingExtensions.SystemSetting.EnableDetailedRequestResponseLog);
            
            if (!enableDetailedLog)
            {
                await next(context);
                return;
            }

            // 只对特定的API端点进行详细日志记录
            if (!ShouldLogRequest(context.Request.Path))
            {
                await next(context);
                return;
            }

            await ProcessWithDetailedLogging(context, next);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "详细请求响应日志中间件发生错误");
            // 确保请求继续处理，即使日志记录失败
            await next(context);
        }
    }

    private async Task ProcessWithDetailedLogging(HttpContext context, RequestDelegate next)
    {
        var requestBody = await CaptureRequestBodyAsync(context.Request);
        var requestHeaders = CaptureRequestHeaders(context.Request);
        var originalResponseBody = context.Response.Body;
        
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        string responseBody = string.Empty;
        string responseHeaders = string.Empty;

        try
        {
            await next(context);

            responseBody = await CaptureResponseBodyAsync(responseBodyStream);
            responseHeaders = CaptureResponseHeaders(context.Response);
            
            // 记录详细日志到数据库（异步执行，不阻塞响应）
            _ = Task.Run(async () => await LogToDatabase(context, requestBody, responseBody, requestHeaders, responseHeaders));
            
            // 将响应写回原始流
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理请求时发生错误，路径: {Path}", context.Request.Path);
            
            // 即使发生异常也要记录详细日志
            responseBody = $"Error: {ex.Message}";
            responseHeaders = CaptureResponseHeaders(context.Response);
            _ = Task.Run(async () => await LogToDatabase(context, requestBody, responseBody, requestHeaders, responseHeaders));
            
            throw;
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }

    private static bool ShouldLogRequest(PathString path)
    {
        // 只对以下API端点进行详细日志记录
        var logPaths = new[]
        {
            "/v1/chat/completions",
            "/v1/completions", 
            "/v1/embeddings",
            "/v1/images/generations",
            "/v1/audio/transcriptions",
            "/v1/audio/translations",
            "/v1/responses"
        };

        return logPaths.Any(p => path.StartsWithSegments(p));
    }

    private static async Task<string> CaptureRequestBodyAsync(HttpRequest request)
    {
        if (request.Body == null || !request.Body.CanRead)
            return string.Empty;

        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        // 限制日志大小，避免过大的请求体
        return body.Length > 10000 ? body[..10000] + "...[truncated]" : body;
    }

    private static async Task<string> CaptureResponseBodyAsync(MemoryStream responseBodyStream)
    {
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        // 限制日志大小，避免过大的响应体
        return body.Length > 10000 ? body[..10000] + "...[truncated]" : body;
    }

    private static string CaptureRequestHeaders(HttpRequest request)
    {
        try
        {
            var headers = request.Headers
                .Where(h => !IsSensitiveHeader(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.ToString());
            
            return JsonSerializer.Serialize(headers);
        }
        catch
        {
            return "{}";
        }
    }

    private static string CaptureResponseHeaders(HttpResponse response)
    {
        try
        {
            var headers = response.Headers
                .ToDictionary(h => h.Key, h => h.Value.ToString());
            
            return JsonSerializer.Serialize(headers);
        }
        catch
        {
            return "{}";
        }
    }

    private async Task LogToDatabase(HttpContext context, string requestBody, string responseBody, 
        string requestHeaders, string responseHeaders)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var loggerService = scope.ServiceProvider.GetRequiredService<LoggerService>();
            
            // 获取用户和Token信息
            var userId = context.User?.FindFirst("uid")?.Value ?? "anonymous";
            var userName = context.User?.FindFirst("username")?.Value ?? "unknown";
            var tokenName = ExtractTokenFromHeader(context.Request.Headers["Authorization"].FirstOrDefault());

            var metadata = new Dictionary<string, string>
            {
                ["DetailedLogging"] = "true",
                ["RequestPath"] = context.Request.Path,
                ["RequestMethod"] = context.Request.Method,
                ["StatusCode"] = context.Response.StatusCode.ToString(),
                ["UserAgent"] = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "",
                ["ContentType"] = context.Request.ContentType ?? "",
                ["RequestSize"] = requestBody?.Length.ToString() ?? "0",
                ["ResponseSize"] = responseBody?.Length.ToString() ?? "0"
            };

            // 创建详细日志记录
            var logger = new ChatLogger
            {
                Type = ThorChatLoggerType.Consume,
                Content = $"详细请求日志 - {context.Request.Method} {context.Request.Path}",
                ModelName = "system-detailed-log",
                PromptTokens = 0,
                CompletionTokens = 0,
                Quota = 0,
                TokenName = tokenName,
                UserName = userName,
                UserId = userId,
                IP = GetClientIP(context),
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                TotalTime = 0,
                Stream = false,
                IsSuccess = context.Response.StatusCode < 400,
                Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                Metadata = metadata,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                RequestHeaders = requestHeaders,
                ResponseHeaders = responseHeaders
            };

            await loggerService.CreateAsync(logger);

            _logger.LogDebug("详细请求响应日志已记录 - 用户: {UserId}({UserName}), 路径: {Path}, 状态码: {StatusCode}",
                userId, userName, context.Request.Path, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "记录详细请求响应日志到数据库时发生错误");
        }
    }

    private static string? ExtractTokenFromHeader(string? authHeader)
    {
        if (string.IsNullOrEmpty(authHeader))
            return null;
            
        const string bearerPrefix = "Bearer ";
        if (authHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader[bearerPrefix.Length..];
            // 对token进行脱敏处理，只显示前3位和后3位
            return token.Length > 6 ? $"{token[..3]}...{token[^3..]}" : token;
        }
        
        return authHeader;
    }

    private static string GetClientIP(HttpContext context)
    {
        // 尝试获取真实客户端IP
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static bool IsSensitiveHeader(string headerName)
    {
        var sensitiveHeaders = new[]
        {
            "authorization",
            "cookie",
            "x-api-key",
            "api-key",
            "x-forwarded-for",
            "x-real-ip"
        };

        return sensitiveHeaders.Contains(headerName.ToLowerInvariant());
    }
}