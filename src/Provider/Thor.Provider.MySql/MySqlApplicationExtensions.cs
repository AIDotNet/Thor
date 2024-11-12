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
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));


        services.AddLocalDataAccess<MySqlLoggerContext>(((provider, builder) =>
        {
            builder.UseMySql(configuration.GetConnectionString("LoggerConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("LoggerConnection")));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        return services;
    }
}