using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Thor.Service.Domain;

namespace Thor.Service.DataAccess;

public static class EntityConfigExtensions
{
    private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static ModelBuilder ConfigureAIDotNet(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.UserName).IsRequired();

            options.Property(x => x.Email).IsRequired();
        });

        modelBuilder.Entity<Token>(options =>
        {
            options.HasKey(x => x.Id);

            options
                .Property(e => e.Id).ValueGeneratedOnAdd();

            options.Property(x => x.Key).IsRequired();

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Key);

            options.Property(x => x.Key).HasMaxLength(42);
        });

        modelBuilder.Entity<RedeemCode>(options =>
        {
            options.HasKey(x => x.Id);

            options
                .Property(e => e.Id).ValueGeneratedOnAdd();

            options.Property(x => x.Name).IsRequired();

            options.Property(x => x.Code).IsRequired();

            options.HasIndex(x => x.Code);
        });

        modelBuilder.Entity<Setting>(options =>
        {
            options.HasKey(x => x.Key);

            options.Property(x => x.Key).IsRequired();

            options.Property(x => x.Value).IsRequired();
        });

        modelBuilder.Entity<ChatChannel>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Name);

            options.Property(x => x.Models)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<List<string>>(item, _jsonSerializerOptions));

            options.Property(x => x.Extension)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<Dictionary<string, string>>(item, _jsonSerializerOptions));
        });

        modelBuilder.Entity<ProductPurchaseRecord>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.UserId);
        });

        modelBuilder.Entity<Product>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Name).IsRequired();
        });

        modelBuilder.Entity<RateLimitModel>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Name)
                .IsRequired();

            options.HasIndex(x => x.Model);

            options.HasIndex(x => x.Creator);

            options.Property(x => x.WhiteList)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<List<string>>(item, _jsonSerializerOptions));

            options.Property(x => x.BlackList)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<List<string>>(item, _jsonSerializerOptions));

            options.Property(x => x.Model)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<string[]>(item, _jsonSerializerOptions));
        });

        modelBuilder.Entity<ModelManager>(options =>
        {
            options.HasKey(x => x.Id);

            options.Property(x => x.Model).IsRequired();
            
            options.HasIndex(x=>x.Model);

            options.HasIndex(x => x.Creator);

            options.Property(x => x.Tags)
                .HasConversion(item => JsonSerializer.Serialize(item, _jsonSerializerOptions),
                    item => JsonSerializer.Deserialize<List<string>>(item, _jsonSerializerOptions));
        });

        return modelBuilder;
    }

    public static ModelBuilder ConfigureLogger(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StatisticsConsumesNumber>(options =>
        {
            options.HasKey(x => x.Id);

            options
                .Property(e => e.Id).ValueGeneratedOnAdd();

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Year);

            options.HasIndex(x => x.Month);

            options.HasIndex(x => x.Day);
        });

        modelBuilder.Entity<ModelStatisticsNumber>(options =>
        {
            options.HasKey(x => x.Id);
            options
                .Property(e => e.Id).ValueGeneratedOnAdd();

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.ModelName);

            options.HasIndex(x => x.Year);

            options.HasIndex(x => x.Month);

            options.HasIndex(x => x.Day);
        });

        modelBuilder.Entity<ChatLogger>(options =>
        {
            options.HasKey(x => x.Id);
            options
                .Property(e => e.Id).ValueGeneratedOnAdd();

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.TokenName);

            options.HasIndex(x => x.ModelName);

            options.HasIndex(x => x.UserName);
        });


        return modelBuilder;
    }
}