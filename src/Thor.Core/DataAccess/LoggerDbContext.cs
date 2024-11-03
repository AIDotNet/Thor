using Microsoft.EntityFrameworkCore;
using Thor.Core.DataAccess;
using Thor.Service.DataAccess;
using Thor.Service.Domain;

namespace Thor.Core;

public class LoggerDbContext<TContext>(DbContextOptions<TContext> options, IServiceProvider serviceProvider)
    : BaseContext<TContext>(options, serviceProvider), ILoggerDbContext where TContext : BaseContext<TContext>
{
    public DbSet<ChatLogger> Loggers { get; set; }

    public DbSet<StatisticsConsumesNumber> StatisticsConsumesNumbers { get; set; }

    public DbSet<ModelStatisticsNumber> ModelStatisticsNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureLogger();
    }

}