using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Thor.Core.DataAccess;
using Thor.Core.Extensions;

namespace Thor.Provider;

public static class SqliteApplicationExtensions
{
    public static IServiceCollection AddThorSqliteDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddThorDataAccess<SqliteThorContext>(((provider, builder) =>
        {
            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));


        services.AddLocalDataAccess<SqliteLoggerContext>(((provider, builder) =>
        {
            builder.UseSqlite(configuration.GetConnectionString("LoggerConnection"));
            // sql日志不输出控制台
            builder.UseLoggerFactory(LoggerFactory.Create(_ => { }));
        }));

        return services;
    }
}