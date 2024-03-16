using AIDotNet.API.Service.Domina;
using AIDotNet.API.Service.Domina.Core;
using AIDotNet.API.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TokenApi.Contract.Domain;

namespace AIDotNet.API.Service.DataAccess;

public sealed class TokenApiDbContext(
    DbContextOptions<TokenApiDbContext> options,
    IUserContext userContext) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<Logger> Loggers { get; set; }

    public DbSet<ChatChannel> Channels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TokenApiDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.UserName).IsRequired();

            options.Property(x => x.Email).IsRequired();
        });

        modelBuilder.Entity<Token>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Key).IsRequired();

            options.HasIndex(x => x.Creator);

            options.Property(x => x.Key).HasMaxLength(42);
        });

        modelBuilder.Entity<Logger>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.TokenName);
        });

        modelBuilder.Entity<ChatChannel>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Name);
        });

        var user = new User(Guid.NewGuid().ToString(), "admin", "239573049@qq.com", "admin");

        modelBuilder.Entity<User>().HasData(user);
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
                        creatable.CreatedAt = DateTime.UtcNow;
                        creatable.Creator = userContext.CurrentUserId;
                        break;
                    case { State: EntityState.Modified, Entity: IUpdatable entity }:
                        entity.UpdatedAt = DateTime.UtcNow;
                        entity.Modifier = userContext.CurrentUserId;
                        break;
                }
            }
            else
            {
                switch (entry.Entity)
                {
                    case ICreatable creatable:
                        creatable.CreatedAt = DateTime.UtcNow;
                        break;
                    case IUpdatable entity:
                        entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}