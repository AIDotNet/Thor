using System.Text.Json;
using AIDotNet.API.Service.Domain;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.DataAccess;

public static class EntityConfigExtensions
{
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

            options.Property(x => x.Key).IsRequired();

            options.HasIndex(x => x.Creator);

            options.Property(x => x.Key).HasMaxLength(42);
        });

        modelBuilder.Entity<ChatLogger>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.TokenName);

            options.HasIndex(x => x.ModelName);

            options.HasIndex(x => x.UserName);
        });

        modelBuilder.Entity<ChatChannel>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Name);

            options.Property(x => x.Models)
                .HasConversion(item => JsonSerializer.Serialize(item, new JsonSerializerOptions()),
                    item => JsonSerializer.Deserialize<List<string>>(item, new JsonSerializerOptions()));

            options.Property(x => x.Extension)
                .HasConversion(item => JsonSerializer.Serialize(item, new JsonSerializerOptions()),
                    item => JsonSerializer.Deserialize<Dictionary<string, string>>(item, new JsonSerializerOptions()));
        });

        modelBuilder.Entity<RedeemCode>(options =>
        {
            options.HasKey(x => x.Id);

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

        modelBuilder.Entity<StatisticsConsumesNumber>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.Year);

            options.HasIndex(x => x.Month);

            options.HasIndex(x => x.Day);
        });

        modelBuilder.Entity<ModelStatisticsNumber>(options =>
        {
            options.HasKey(x => x.Id);

            options.HasIndex(x => x.Creator);

            options.HasIndex(x => x.ModelName);

            options.HasIndex(x => x.Year);

            options.HasIndex(x => x.Month);

            options.HasIndex(x => x.Day);
        });

        return modelBuilder;
    }
}