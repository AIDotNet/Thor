using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }));


        services.AddLocalDataAccess<PostgreSQLLoggerContext>(((provider, builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("LoggerConnection"));
        }));

        return services;
    }
}