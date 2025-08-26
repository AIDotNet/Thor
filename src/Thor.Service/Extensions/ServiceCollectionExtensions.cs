using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using Thor.Service.Infrastructure;
using Thor.Service.Options;
using Thor.Service.Infrastructure.Middlewares;

namespace Thor.Service.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册自定义认证
    /// </summary>
    /// <param Name="services"></param>
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        //使用应用密钥得到一个加密密钥字节数组
        services
            .AddAuthorization()
            .AddAuthentication("CustomAuthentication")
            .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuthentication", null);

        return services;
    }


    public static IEndpointRouteBuilder UseOpenTelemetry(this WebApplication app)
    {
        app.Use((async (context, next) =>
        {
            context.Response.Headers.TryAdd<string, StringValues>("X-Request-Id",
                (StringValues)Activity.Current?.TraceId.ToString());

            Activity.Current?.SetTag("http.method", context.Request.Method);
            Activity.Current?.SetTag("http.url", context.Request.Path);
            Activity.Current?.SetTag("http.host", context.Request.Host.ToString());

            await next(context);
        }));
        return app;
    }

    public static WebApplicationBuilder HostEnvironment(this WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(JwtOptions.Name)
            .Get<JwtOptions>();

        builder.Configuration.GetSection(CacheOptions.Name)
            .Get<CacheOptions>();
        builder.Configuration.GetSection(ChatCoreOptions.Name)
            .Get<ChatCoreOptions>();
        builder.Configuration.GetSection(TrackerOptions.Tracker)
            .Get<TrackerOptions>();
        
        // 添加TracingOptions配置
        builder.Services.Configure<TracingOptions>(
            builder.Configuration.GetSection(TracingOptions.SectionName));

        var cacheType = Environment.GetEnvironmentVariable("CACHE_TYPE");
        var connectionString = Environment.GetEnvironmentVariable("CACHE_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(cacheType))
        {
            CacheOptions.Type = cacheType;
        }

        if (!string.IsNullOrEmpty(connectionString))
        {
            CacheOptions.ConnectionString = connectionString;
        }
        
        // var ConnectionStrings:DefaultConnection
        var defaultConnection = builder.Configuration["DefaultConnection"];
        var loggerConnection = builder.Configuration["LoggerConnection"];
        
        if (!string.IsNullOrEmpty(defaultConnection))
        {
            // 修改默认连接字符串
            builder.Configuration["ConnectionStrings:DefaultConnection"] = defaultConnection;
        }
        
        if (!string.IsNullOrEmpty(loggerConnection))
        {
            // 修改日志连接字符串
            builder.Configuration["ConnectionStrings:LoggerConnection"] = loggerConnection;
        }

        return builder;
    }
}