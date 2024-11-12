using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class DMApplicationExtensions
{
    public static IServiceCollection AddThorDMDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddThorDataAccess<DMThorContext>(((provider, builder) =>
        {
            builder.UseDm(configuration.GetConnectionString("DefaultConnection"));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));


        services.AddLocalDataAccess<DMLoggerContext>(((provider, builder) =>
        {
            builder.UseDm(configuration.GetConnectionString("LoggerConnection"));
            
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        return services;
    }
}