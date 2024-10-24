using System.Diagnostics;

namespace Thor.Service.Infrastructure.Middlewares;

public class OpenTelemetryMiddleware : IMiddleware, ISingletonDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using var activity = new Activity("Thor.Service");

        // 设置链路追踪Id
        if (context.Request.Headers.TryGetValue("traceparent", out var traceparent))
        {
            activity.SetParentId(traceparent.ToString());
        }
        else
        {
            activity.SetParentId(ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom());
        }

        activity.Start();

        await next(context);

        activity.Stop();
    }
}