using Thor.Core.DataAccess;

namespace Thor.Service.Extensions;

public static class EntityFrameworkCoreExtensions
{
    /// <summary>
    /// 迁移数据库
    /// </summary>
    /// <returns></returns>
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        // 获取配置，判断是否需要迁移
        var runMigrationsAtStartup = app.Configuration.GetValue<bool>("RunMigrationsAtStartup");

        if (!runMigrationsAtStartup)
        {
            return app;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IThorContext>();
        var loggerContext = scope.ServiceProvider.GetRequiredService<ILoggerDbContext>();

        await dbContext.Database.MigrateAsync();
        await loggerContext.Database.MigrateAsync();

        return app;
    }
}