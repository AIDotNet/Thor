using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Thor.Domain.Chats;
using Thor.Service.Domain;

namespace Thor.Core.DataAccess;

public interface ILoggerDbContext
{
    DatabaseFacade Database { get; }
    
    DbSet<ChatLogger> Loggers { get; set; }

    DbSet<StatisticsConsumesNumber> StatisticsConsumesNumbers { get; set; }

    DbSet<ModelStatisticsNumber> ModelStatisticsNumbers { get; set; }

    /// <summary>
    /// 链路追踪
    /// </summary>
    DbSet<Tracing> Tracings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

}