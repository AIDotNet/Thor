using Microsoft.EntityFrameworkCore;
using Thor.Service.Domain;

namespace Thor.Core.DataAccess;

public interface IThorContext
{
    DbSet<User> Users { get; set; }

    DbSet<Token> Tokens { get; set; }

    DbSet<ChatChannel> Channels { get; set; }

    DbSet<RedeemCode> RedeemCodes { get; set; }

    DbSet<Setting> Settings { get; set; }

    DbSet<Product> Products { get; set; }

    DbSet<ProductPurchaseRecord> ProductPurchaseRecords { get; set; }

    DbSet<RateLimitModel> RateLimitModels { get; set; }

    DbSet<ModelManager> ModelManagers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    Task RunMigrationsAsync(CancellationToken cancellationToken = new CancellationToken());
}