using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Thor.Core.DataAccess;
using Thor.Service.DataAccess;
using Thor.Service.Domain;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Core;

public abstract class ThorContext<TContext>(DbContextOptions<TContext> context, IServiceProvider serviceProvider)
    : BaseContext<TContext>(context, serviceProvider), IThorContext where TContext : BaseContext<TContext>
{
    public DbSet<User> Users { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<ChatChannel> Channels { get; set; }

    public DbSet<RedeemCode> RedeemCodes { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductPurchaseRecord> ProductPurchaseRecords { get; set; }

    public DbSet<RateLimitModel> RateLimitModels { get; set; }

    public DbSet<ModelManager> ModelManagers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureAIDotNet();

        var user = new User(Guid.NewGuid().ToString(), "admin", "239573049@qq.com", "admin")
        {
            CreatedAt = DateTime.Now
        };
        user.SetAdmin();
        user.SetResidualCredit(1000000000);

        modelBuilder.Entity<User>().HasData(user);

        var token = new Token
        {
            Id = Guid.NewGuid().ToString(),
            Key = "sk-" + StringHelper.GenerateRandomString(38),
            Creator = user.Id,
            Name = "默认Token",
            UnlimitedQuota = true,
            UnlimitedExpired = true,
            CreatedAt = DateTime.Now
        };

        modelBuilder.Entity<Token>().HasData(token);

        modelBuilder.InitSetting();


        if (!File.Exists("model-manager.json")) return;

        var modelManagers = new List<ModelManager>();
        try
        {
            var json = File.ReadAllText("model-manager.json");

            modelManagers.AddRange(JsonSerializer.Deserialize<List<ModelManager>>(json, new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }) ?? []);

            modelManagers.ForEach(x =>
            {
                x.Id = Guid.NewGuid();
                x.CreatedAt = DateTime.Now;
                x.Enable = true;
            });

            modelBuilder.Entity<ModelManager>().HasData(modelManagers);
        }
        catch (Exception e)
        {
            Console.WriteLine("模型默认配置文件错误：" + e);
        }
    }
}