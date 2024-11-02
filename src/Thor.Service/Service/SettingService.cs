using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Thor.Core.DataAccess;
using Thor.Service.Infrastructure;

namespace Thor.Service.Service;

public static class SettingService
{
    // 超级轻量级的集合，高性能查询
    private static ImmutableList<Setting> Settings { get; set; } = ImmutableList<Setting>.Empty;

    public static async Task LoadingSettings(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IThorContext>();
        var settings = await dbContext.Settings.ToListAsync();
        Settings = settings.ToImmutableList();
        
        await ModelManagerService.LoadingSettings(dbContext);
    }

    public static string GetSetting(string key)
    {
        return Settings.FirstOrDefault(x => x.Key == key)?.Value ?? string.Empty;
    }

    public static T GetSetting<T>(string key)
    {
        var value = GetSetting(key);
        return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public static int GetIntSetting(string key)
    {
        var value = GetSetting(key);
        return string.IsNullOrEmpty(value) ? 0 : int.TryParse(value, out var result) ? result : 0;
    }

    public static bool GetBoolSetting(string key)
    {
        var value = GetSetting(key);
        return !string.IsNullOrEmpty(value) && bool.TryParse(value, out var result) && result;
    }

    /// <summary>
    ///     如果是管理员，返回所有设置，否则返回公开的设置
    /// </summary>
    /// <param name="userContext"></param>
    /// <returns></returns>
    public static ImmutableList<Setting> GetSettings(IUserContext userContext)
    {
        return userContext is { IsAuthenticated: true, IsAdmin: true }
            ? Settings
            : Settings.Where(x => x.Private == false).ToImmutableList();
    }

    /// <summary>
    ///     更新设置
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="dbContext"></param>
    public static async ValueTask UpdateSettingsAsync([FromBody] List<Setting> settings,
        IThorContext dbContext)
    {
        var dbSettings = await dbContext.Settings.ToListAsync();
        foreach (var setting in dbSettings)
        {
            var newSetting = settings.FirstOrDefault(x => x.Key == setting.Key);
            if (newSetting != null) setting.Value = newSetting.Value;
        }

        dbContext.Settings.UpdateRange(dbSettings);

        await dbContext.SaveChangesAsync();

        Settings = dbSettings.ToImmutableList();
    }
}