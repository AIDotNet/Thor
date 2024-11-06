using System.Diagnostics;

namespace Thor.Service.Infrastructure.Middlewares;

public class OpenTelemetryMiddlewares : IMiddleware, ISingletonDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 如果是Get则跳过
        if (context.Request.Method is "GET" or "OPTIONS" or "HEAD" or "TRACE" or "CONNECT")
        {
            await next(context);
            return;
        }

        using var consume =
            Activity.Current?.Source.StartActivity("Request Start", ActivityKind.Server);

        consume?.SetTag("RequestPath", context.Request.Path);
        consume?.SetTag("RequestMethod", context.Request.Method);
        consume?.SetTag("RequestHost", context.Request.Host);
        consume?.SetTag("RequestScheme", context.Request.Scheme);
        consume?.SetTag("RequestQueryString", context.Request.QueryString);
        consume?.SetStartTime(DateTime.UtcNow);

        await next(context);

        consume?.SetTag("ResponseStatusCode", context.Response.StatusCode);

        consume?.SetTag("Request End", "End");

        consume?.SetEndTime(DateTime.UtcNow);
    }
}