using Microsoft.EntityFrameworkCore;
using Thor.Service.Domain;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure;

namespace Thor.Service.DataAccess;

public class LoggerDbContext(
    DbContextOptions<LoggerDbContext> options,
    IUserContext userContext) : DbContext(options)
{
    public DbSet<ChatLogger> Loggers { get; set; }

    public DbSet<StatisticsConsumesNumber> StatisticsConsumesNumbers { get; set; }

    public DbSet<ModelStatisticsNumber> ModelStatisticsNumbers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureLogger();
    }


    public override int SaveChanges()
    {
        OnBeforeSaveChanges();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (userContext.IsAuthenticated)
            {
                switch (entry)
                {
                    case { State: EntityState.Added, Entity: ICreatable creatable }:
                        creatable.Creator ??= userContext.CurrentUserId;
                        if (creatable.CreatedAt == default)
                            creatable.CreatedAt = DateTime.Now;
                        break;
                    case { State: EntityState.Modified, Entity: IUpdatable entity }:
                        entity.UpdatedAt ??= DateTime.Now;
                        entity.Modifier ??= userContext.CurrentUserId;
                        break;
                }
            }
            else
            {
                switch (entry.Entity)
                {
                    case ICreatable creatable:
                        if (creatable.CreatedAt == default)
                            creatable.CreatedAt = DateTime.Now;
                        break;
                    case IUpdatable entity:
                        entity.UpdatedAt ??= DateTime.Now;
                        break;
                }
            }
        }
    }
}