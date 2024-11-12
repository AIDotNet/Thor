using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class PostgreSQLApplicationExtensions
{
    public static IServiceCollection AddThorPostgreSQLDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddThorDataAccess<PostgreSQLThorContext>(((provider, builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        services.AddLocalDataAccess<PostgreSQLLoggerContext>(((provider, builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("LoggerConnection"));
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        return services;
    }
}