using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Thor.Core.DataAccess;
using Thor.Domain.Chats;
using Thor.Domain.Users;
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

    public DbSet<ModelMap> ModelMaps { get; set; }
    
    public DbSet<UserGroup> UserGroups { get; set; }
    
    public DbSet<Announcement> Announcements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureAIDotNet();

        var user = new User("CA378C74-19E7-458A-918B-4DBB7AE1729D", "admin", "239573049@qq.com", "admin")
        {
            CreatedAt = DateTime.Now,
            Groups = ["default", "vip"],
        };
        user.SetAdmin();
        user.SetResidualCredit(1000000000);

        modelBuilder.Entity<User>().HasData(user);

        var token = new Token
        {
            Id = "CA378C74-19E7-458A-918B-4DBB7AE1729D",
            Key = "sk-" + StringHelper.GenerateRandomString(38),
            Creator = user.Id,
            Name = "默认Token",
            UnlimitedQuota = true,
            UnlimitedExpired = true,
            CreatedAt = DateTime.Now,
            Groups = ["default"],
        };

        modelBuilder.Entity<Token>().HasData(token);

        modelBuilder.InitSetting();

        var userGroups = new List<UserGroup>
        {
            new()
            {
                Id = Guid.Parse("CA378C74-19E7-458A-918B-4DBB7AE1729D"),
                Code = "default",
                Name = "默认",
                Description = "默认用户组",
                CreatedAt = DateTime.Now,
                Enable = true,
                Rate = 1,
            },
            new()
            {
                Id = Guid.Parse("CA378C74-19E7-458A-918B-4DBB7AE17291"),
                Code = "vip",
                Name = "VIP",
                Description = "VIP用户组",
                CreatedAt = DateTime.Now,
                Enable = true,
                Rate = 1,
            }
        };

        modelBuilder.Entity<UserGroup>().HasData(userGroups);

        modelBuilder.Entity<ModelMap>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Id).ValueGeneratedOnAdd();

            options.Property(x => x.ModelId).IsRequired();

            options.HasIndex(x => x.ModelId);

            options.Property(x => x.ModelMapItems)
                .HasConversion((list => JsonSerializer.Serialize(list, JsonSerializerOptions.Web)),
                    (str => JsonSerializer.Deserialize<List<ModelMapItem>>(str, JsonSerializerOptions.Web) ??
                            new List<ModelMapItem>()));

            options.Property(x => x.Group)
                .HasConversion((x) => JsonSerializer.Serialize(x, JsonSerializerOptions.Web),
                    (x) => JsonSerializer.Deserialize<string[]>(x, JsonSerializerOptions.Web) ?? Array.Empty<string>());
        });

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