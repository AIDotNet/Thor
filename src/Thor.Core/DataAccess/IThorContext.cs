using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Thor.Domain.Users;
using Thor.Service.Domain;

namespace Thor.Core.DataAccess;

public interface IThorContext
{
    DatabaseFacade Database { get; }

    DbSet<User> Users { get; set; }

    DbSet<Token> Tokens { get; set; }

    DbSet<ChatChannel> Channels { get; set; }

    DbSet<RedeemCode> RedeemCodes { get; set; }

    DbSet<Setting> Settings { get; set; }

    DbSet<Product> Products { get; set; }

    DbSet<ProductPurchaseRecord> ProductPurchaseRecords { get; set; }

    DbSet<RateLimitModel> RateLimitModels { get; set; }

    DbSet<ModelManager> ModelManagers { get; set; }

    DbSet<ModelMap> ModelMaps { get; set; }

    DbSet<UserGroup> UserGroups { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}