using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }));


        services.AddLocalDataAccess<DMLoggerContext>(((provider, builder) =>
        {
            builder.UseDm(configuration.GetConnectionString("LoggerConnection"));
        }));

        return services;
    }
}