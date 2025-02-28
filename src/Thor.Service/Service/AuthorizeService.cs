using System.Net.Http.Headers;
using Casdoor.Client;
using Thor.Abstractions.Dtos;
using Thor.BuildingBlocks.Event;
using Thor.Service.Eto;

namespace Thor.Service.Service;

public class AuthorizeService(
    IServiceProvider serviceProvider,
    IEventBus<CreateUserEto> eventBus,
    ILogger<AuthorizeService> logger,
    IConfiguration configuration,
    JwtHelper jwtHelper)
    : ApplicationService(serviceProvider), IScopeDependency
{
    private static readonly HttpClient HttpClient = new(new SocketsHttpHandler
    {
        SslOptions =
        {
            RemoteCertificateValidationCallback = (_, _, _, _) => true
        }
    });

    static AuthorizeService()
    {
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "AIDotNet");
        HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<object> TokenAsync(LoginInput input)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x =>
            x.UserName == input.account || x.Email == input.account);

        if (user == null)
            throw new Exception("账号不存在");

        if (user.IsDisabled)
            throw new Exception("账号已被禁用");

        if (user.Password != StringHelper.HashPassword(input.pass, user.PasswordHas))
            throw new Exception("密码错误");

        var key = jwtHelper.CreateToken(user);

        return new
        {
            token = key,
            role = user.Role
        };
    }

    public async Task<object> GithubAsync(string code)
    {
        var isGithub = SettingService.GetBoolSetting(SettingExtensions.SystemSetting.EnableGithubLogin);

        if (!isGithub) throw new Exception("Github login is not enabled");

        var clientId = SettingService.GetSetting(SettingExtensions.SystemSetting.GithubClientId);
        var clientSecret = SettingService.GetSetting(SettingExtensions.SystemSetting.GithubClientSecret);

        var endpoint = configuration["Github:Endpoint"]?.TrimEnd('/');
        var apiEndpoint = configuration["Github:ApiEndpoint"]?.TrimEnd('/');

        var response =
            await HttpClient.PostAsync(
                $"{endpoint}/login/oauth/access_token?code={code}&client_id={clientId}&client_secret={clientSecret}",
                null);

        logger.LogInformation("Github授权：" + response.StatusCode +
                              $" Endpoint {endpoint} code:{code} clientId={clientId} secret={clientSecret}");

        var result = await response.Content.ReadFromJsonAsync<GitTokenDto>();
        if (result is null)
        {
            logger.LogError("Github授权失败");
            throw new Exception("Github授权失败");
        }

        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{apiEndpoint}/user")
        {
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", result.access_token)
            }
        };

        logger.LogInformation("Github授权：" + result.access_token);

        var responseMessage = await HttpClient.SendAsync(request);

        var githubUser = await responseMessage.Content.ReadFromJsonAsync<GithubUserDto>();
        if (githubUser is null)
        {
            logger.LogError("Github授权失败");
            throw new Exception("Github授权失败");
        }

        if (githubUser.id < 1000)
        {
            logger.LogError("Github授权失败");
            throw new Exception("Github授权失败");
        }

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == githubUser.id.ToString());

        if (user is null)
        {
            user = new User(githubUser.id.ToString(), githubUser.id.ToString(),
                githubUser.id + "@token-ai.cn",
                Guid.NewGuid().ToString("N"));
            
            user.Groups = ["default"];
            
            user.SetUser();

            user.SetPassword("Aa123456");

            await DbContext.Users.AddAsync(user);

            // 初始用户额度
            var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
            user.SetResidualCredit(userQuota);

            await DbContext.Users.AddAsync(user);

            // 发送创建用户事件
            await eventBus.PublishAsync(new CreateUserEto()
            {
                User = user,
                Source = CreateUserSource.Github
            });

            await DbContext.SaveChangesAsync();
        }

        var key = jwtHelper.CreateToken(user);

        return new
        {
            token = key,
            role = user.Role
        };
    }

    public async Task<object> GiteeAsync(string code, string redirectUri)
    {
        var isGitee = SettingService.GetBoolSetting(SettingExtensions.SystemSetting.EnableGithubLogin);

        if (!isGitee) throw new Exception("Gitee 没有启用");

        var clientId = SettingService.GetSetting(SettingExtensions.SystemSetting.GiteeClientId);
        var clientSecret = SettingService.GetSetting(SettingExtensions.SystemSetting.GiteeClientSecret);

        var url =
            $"https://gitee.com/oauth/token?grant_type=authorization_code&redirect_uri={redirectUri}&response_type=code&code={code}&client_id={clientId}&client_secret={clientSecret}";

        var response =
            await HttpClient.PostAsync(
                url,
                null);

        var result = await response.Content.ReadFromJsonAsync<GitTokenDto>();
        if (result is null)
        {
            logger.LogError("Gitee授权失败");
            throw new Exception("Gitee授权失败");
        }

        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://gitee.com/api/v5/user?access_token=" + result.access_token);

        logger.LogInformation("Github授权：" + result.access_token);

        var responseMessage = await HttpClient.SendAsync(request);

        var githubUser = await responseMessage.Content.ReadFromJsonAsync<GithubUserDto>();
        if (githubUser is null)
        {
            logger.LogError("Gitee授权失败");
            throw new Exception("Gitee授权失败");
        }

        if (githubUser.id < 1000)
        {
            logger.LogError("Gitee授权失败");
            throw new Exception("Gitee授权失败");
        }

        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == githubUser.id.ToString());

        if (user is null)
        {
            user = new User(githubUser.id.ToString(), githubUser.id.ToString(),
                githubUser.id + "@token-ai.cn",
                Guid.NewGuid().ToString("N"));
            user.SetUser();

            user.Groups = ["default"];

            user.SetPassword("Aa123456");

            await DbContext.Users.AddAsync(user);

            // 初始用户额度
            var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
            user.SetResidualCredit(userQuota);

            await DbContext.Users.AddAsync(user);

            await eventBus.PublishAsync(new CreateUserEto()
            {
                User = user,
                Source = CreateUserSource.Gitee
            });

            await DbContext.SaveChangesAsync();
        }

        var key = jwtHelper.CreateToken(user);

        return new
        {
            token = key,
            role = user.Role
        };
    }

    public async Task<object> CasdoorAsync(string code)
    {
        var enable = SettingService.GetBoolSetting(SettingExtensions.SystemSetting.EnableCasdoorAuth);

        if (!enable) throw new Exception("Casdoor 没有启用");

        var clientId = SettingService.GetSetting(SettingExtensions.SystemSetting.CasdoorClientId);
        var endipoint = SettingService.GetSetting(SettingExtensions.SystemSetting.CasdoorEndipoint);
        var clientSecret = SettingService.GetSetting(SettingExtensions.SystemSetting.CasdoorClientSecret);


        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", code }
        };
        var content = new FormUrlEncodedContent(parameters);

        var url = $"{endipoint.TrimEnd('/')}/api/login/oauth/access_token";

        var response = await HttpClient.PostAsync(url, content);

        var str = await response.Content.ReadFromJsonAsync<GitTokenDto>();

        if (str is null)
        {
            logger.LogError("Casdoor授权失败");
            throw new Exception("Casdoor授权失败");
        }

        var casdoorUser =
            await HttpClient.GetFromJsonAsync<CasdoorUserDto>($"{endipoint.TrimEnd('/')}/api/get-account?accessToken=" +
                                                              str.access_token);

        if (casdoorUser is null)
        {
            logger.LogError("Casdoor授权失败");
            throw new Exception("Casdoor授权失败");
        }


        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == casdoorUser.Data.id);

        if (user is null)
        {
            user = new User(casdoorUser.Data.id, casdoorUser.Data.id,
                casdoorUser.Data.id + "@token-ai.cn",
                Guid.NewGuid().ToString("N"));
            user.SetUser();
            user.Groups = ["default"];


            user.SetPassword("Aa123456");

            await DbContext.Users.AddAsync(user);

            // 初始用户额度
            var userQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.NewUserQuota);
            user.SetResidualCredit(userQuota);

            await DbContext.Users.AddAsync(user);

            await eventBus.PublishAsync(new CreateUserEto()
            {
                User = user,
                Source = CreateUserSource.Gitee
            });

            await DbContext.SaveChangesAsync();
        }

        var key = jwtHelper.CreateToken(user);

        return new
        {
            token = key,
            role = user.Role
        };
    }
}