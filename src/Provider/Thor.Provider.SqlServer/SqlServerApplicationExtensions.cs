using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class SqlServerApplicationExtensions
{
    public static IServiceCollection AddThorSqlServerDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddThorDataAccess<SqlServerThorContext>(((provider, builder) =>
        {
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }));


        services.AddLocalDataAccess<SqlServerLoggerContext>(((provider, builder) =>
        {
            builder.UseSqlServer(configuration.GetConnectionString("LoggerConnection"));
        }));

        return services;
    }
}