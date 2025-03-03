using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class MySqlApplicationExtensions
{
    public static IServiceCollection AddThorMySqlDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddThorDataAccess<MySqlThorContext>(((provider, builder) =>
        {
            builder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.Parse("8.0.0"));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));


        services.AddLocalDataAccess<MySqlLoggerContext>(((provider, builder) =>
        {
            builder.UseMySql(configuration.GetConnectionString("LoggerConnection"),
                ServerVersion.Parse("8.0.0"));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        return services;
    }
}