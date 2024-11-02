using Microsoft.EntityFrameworkCore;
using Thor.Service.Domain;

namespace Thor.Core.DataAccess;

public interface ILoggerDbContext
{
    DbSet<ChatLogger> Loggers { get; set; }

    DbSet<StatisticsConsumesNumber> StatisticsConsumesNumbers { get; set; }

    DbSet<ModelStatisticsNumber> ModelStatisticsNumbers { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    Task RunMigrationsAsync(CancellationToken cancellationToken = new CancellationToken());
}