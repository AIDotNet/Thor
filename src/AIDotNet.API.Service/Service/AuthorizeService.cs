using System.Net.Http.Headers;
using AIDotNet.API.Service.DataAccess;
using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AIDotNet.API.Service.Service;

public sealed class AuthorizeService(
    IServiceProvider serviceProvider,
    LoggerService loggerService,
    TokenService tokenService,
    IMemoryCache memoryCache,
    IHttpClientFactory httpClientFactory)
    : ApplicationService(serviceProvider)
{
    public async Task<object> TokenAsync(LoginInput input)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x =>
            x.UserName == input.account || x.Email == input.account);

        if (user == null)
        {
            throw new Exception("Account does not exist");
        }

        if (user.IsDisabled)
        {
            throw new Exception("Account is disabled");
        }

        if (user.Password != StringHelper.HashPassword(input.pass, user.PasswordHas))
        {
            throw new Exception("Password error");
        }

        var key = "su-" + StringHelper.GenerateRandomString(38);

        memoryCache.Set(key, user, TimeSpan.FromDays(7));

        return new
        {
            token = key,
            role = user.Role
        };
    }

    public async Task<object> GithubAsync(string code)
    {
        var isGithub = SettingService.GetBoolSetting(SettingExtensions.SystemSetting.EnableGithubLogin);

        if (!isGithub)
        {
            throw new Exception("Github login is not enabled");
        }

        var apiHttpClient = httpClientFactory.CreateClient("GitHubAuthApi");

        var clientId = SettingService.GetSetting(SettingExtensions.SystemSetting.GithubClientId);
        var clientSecret = SettingService.GetSetting(SettingExtensions.SystemSetting.GithubClientSecret);

        var response =
            await apiHttpClient.PostAsync(
                $"https://github.com/login/oauth/access_token?code={code}&client_id={clientId}&client_secret={clientSecret}",
                null);


        var result = await response.Content.ReadFromJsonAsync<GitTokenDto>();
        if (result is null) throw new Exception("Github授权失败");

        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://api.github.com/user")
        {
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", result.access_token),
            },
        };

        var responseMessage = await apiHttpClient.SendAsync(request);

        var githubUser = await responseMessage.Content.ReadFromJsonAsync<GithubUserDto>();
        if (githubUser is null) throw new Exception("Github授权失败");

        if (githubUser.id < 1000) throw new Exception("Github授权失败");

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == githubUser.id.ToString());

        if (user is null)
        {
            user = new User(githubUser.id.ToString(), githubUser.id.ToString(),
                githubUser.id + "@token-ai.cn",
                Guid.NewGuid().ToString("N"));
            user.SetUser();

            user.SetPassword("Aa123456");

            await DbContext.Users.AddAsync(user);
            
            
            // 初始用户额度
            var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
            user.SetResidualCredit(userQuota);

            await DbContext.Users.AddAsync(user);

            await tokenService.CreateAsync(new TokenInput()
            {
                Name = "默认Token",
                UnlimitedQuota = true,
                UnlimitedExpired = true,
            }, user.Id);

            await loggerService.CreateSystemAsync("Github来源 创建用户：" + user.UserName);

            await DbContext.SaveChangesAsync();
        }

        var key = "su-" + StringHelper.GenerateRandomString(38);

        memoryCache.Set(key, user, TimeSpan.FromDays(7));

        return new
        {
            token = key,
            role = user.Role
        };
    }
}