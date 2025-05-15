using Microsoft.EntityFrameworkCore;
using Thor.Domain.Chats;
using Thor.Service.DataAccess;
using Thor.Service.Domain;

namespace Thor.Core.DataAccess;

public class LoggerDbContext<TContext>(DbContextOptions<TContext> options, IServiceProvider serviceProvider)
    : BaseContext<TContext>(options, serviceProvider), ILoggerDbContext where TContext : BaseContext<TContext>
{
    public DbSet<ChatLogger> Loggers { get; set; }

    public DbSet<StatisticsConsumesNumber> StatisticsConsumesNumbers { get; set; }

    public DbSet<ModelStatisticsNumber> ModelStatisticsNumbers { get; set; }

    public DbSet<Tracing> Tracings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureLogger();
    }

}