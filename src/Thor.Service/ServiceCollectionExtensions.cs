using Microsoft.AspNetCore.Authentication;
using Thor.Service.Infrastructure;
using Thor.Service.Options;

namespace Thor.Service;

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

    
    public static WebApplicationBuilder HostEnvironment(this WebApplicationBuilder builder)
    {
        builder.Configuration.GetSection(JwtOptions.Name)
            .Get<JwtOptions>();

        builder.Configuration.GetSection(CacheOptions.Name)
            .Get<CacheOptions>();
        builder.Configuration.GetSection(ChatCoreOptions.Name)
            .Get<ChatCoreOptions>();
        
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

        return builder;
    }
}