using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class SqliteApplicationExtensions
{
    public static IServiceCollection AddThorSqliteDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddThorDataAccess<SqliteThorContext>(((provider, builder) =>
        {
            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging();
        }));


        services.AddLocalDataAccess<SqliteLoggerContext>(((provider, builder) =>
        {
            builder.UseSqlite(configuration.GetConnectionString("LoggerConnection"))
                .EnableSensitiveDataLogging();
        }));

        return services;
    }
}