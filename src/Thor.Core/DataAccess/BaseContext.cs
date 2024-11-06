using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure;

namespace Thor.Core;

public abstract class BaseContext<TContext>(DbContextOptions<TContext> context, IServiceProvider serviceProvider)
    : DbContext(context) where TContext : DbContext
{
    private readonly IUserContext _userContext = serviceProvider.GetRequiredService<IUserContext>();

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
            if (_userContext.IsAuthenticated)
            {
                switch (entry)
                {
                    case { State: EntityState.Added, Entity: ICreatable creatable }:
                        creatable.Creator ??= _userContext.CurrentUserId;
                        if (creatable.CreatedAt == default)
                            creatable.CreatedAt = DateTime.Now;
                        break;
                    case { State: EntityState.Modified, Entity: IUpdatable entity }:
                        entity.UpdatedAt ??= DateTime.Now;
                        entity.Modifier ??= _userContext.CurrentUserId;
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