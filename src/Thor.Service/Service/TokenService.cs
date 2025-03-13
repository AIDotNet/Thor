using System.Runtime.CompilerServices;
using Thor.Service.Domain.Core;
using Thor.Service.Extensions;

namespace Thor.Service.Service;

public sealed class TokenService(
    IServiceProvider serviceProvider,
    UserService userService,
    JwtHelper jwtHelper,
    ILogger<TokenService> logger)
    : ApplicationService(serviceProvider), IScopeDependency
{
    public async ValueTask<string> CreateAsync(TokenInput input, string? createId = null)
    {
        if (input.ExpiredTime < DateTime.Now) throw new Exception("过期时间不能小于当前时间");

        if (input is { UnlimitedExpired: false, ExpiredTime: null }) throw new Exception("请选择过期时间");

        if (input.Groups.Length <= 0) throw new Exception("请选择分组");

        if (input.Groups.Length > 1) throw new Exception("token只能属于一个分组");

        var token = Mapper.Map<Token>(input);

        if (!string.IsNullOrEmpty(createId)) token.Creator = createId;

        token.Id = Guid.NewGuid().ToString("N");

        token.Key = "sk-" + StringHelper.GenerateRandomString(38);

        await DbContext.Tokens.AddAsync(token);

        return token.Key;
    }

    public async Task<Token?> GetAsync(long id)
    {
        return await DbContext.Tokens
            .FindAsync(id);
    }

    public async ValueTask<PagingDto<Token>> GetListAsync(int page, int pageSize, string? token, string? keyword)
    {
        var query = DbContext.Tokens
            .AsNoTracking()
            .Where(x => x.Creator == UserContext.CurrentUserId)
            .Where(x => string.IsNullOrEmpty(token) || x.Key == token)
            .Where(x => string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword));

        var total = await query
            .CountAsync();

        var items =
            await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return new PagingDto<Token>(total, items);
    }

    public async ValueTask<bool> UpdateAsync(Token input)
    {
        
        if (input.Groups.Length <= 0) throw new Exception("请选择分组");

        if (input.Groups.Length > 1) throw new Exception("token只能属于一个分组");

        
        var result = await DbContext.Tokens.Where(x => x.Id == input.Id)
            .ExecuteUpdateAsync(item =>
                item.SetProperty(x => x.Name, input.Name)
                    .SetProperty(x => x.ExpiredTime, input.ExpiredTime)
                    .SetProperty(x => x.RemainQuota, input.RemainQuota)
                    .SetProperty(x => x.UnlimitedQuota, input.UnlimitedQuota)
                    .SetProperty(x => x.UnlimitedExpired, input.UnlimitedExpired)
                    .SetProperty(x => x.LimitModels, input.LimitModels)
                    .SetProperty(x => x.WhiteIpList, input.WhiteIpList)
                    .SetProperty(x => x.Groups, input.Groups)
            );

        return result > 0;
    }

    public async ValueTask<bool> RemoveAsync(string id)
    {
        var result = await DbContext.Tokens.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async ValueTask DisableAsync(string id)
    {
        await DbContext.Tokens.Where(x => x.Id == id && UserContext.CurrentUserId == x.Creator)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.Disabled, a => !a.Disabled));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CheckModel(string model, Token? token, HttpContext context)
    {
        if (token == null) return;

        if (token.LimitModels.Count(x => !string.IsNullOrEmpty(x)) > 0 && !token.LimitModels.Contains(model))
        {
            // token脱敏
            throw new Exception(
                $"当前 {string.Concat("****", token.Key.AsSpan(token.Key.Length - 5, 5))}Token 无权访问{model}模型,请修改Token权限，当前token可访问模型：{string.Join(",", token.LimitModels)}");
        }

        if (token.WhiteIpList.Count <= 0) return;

        var ip = context.GetIpAddress();

        if (string.IsNullOrEmpty(ip) || !token.WhiteIpList.Contains(ip))
        {
            throw new Exception($"当前IP:{ip}无权访问模型{model}");
        }
    }

    /// <summary>
    ///     校验Token 是否有效
    ///     检验账号额度是否足够
    /// </summary>
    /// <param name="context"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="InsufficientQuotaException"></exception>
    public async ValueTask<(Token?, User)> CheckTokenAsync(HttpContext context, ModelManager value)
    {
        var key = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();

        if (string.IsNullOrEmpty(key))
        {
            var protocol = context.Request.Headers.SecWebSocketProtocol.ToString().Split(",").Select(x => x.Trim());

            var apiKey = protocol
                .FirstOrDefault(x => x.StartsWith("openai-insecure-api-key.", StringComparison.OrdinalIgnoreCase))
                ?.Replace("openai-insecure-api-key.", "");
            if (!string.IsNullOrEmpty(apiKey))
                key = apiKey;
        }

        var requestQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.RequestQuota);

        if (requestQuota <= 0) requestQuota = 5000;

        User? user;
        Token? token;
        // sk-是用户创建的token，否则是用户的JWT
        if (!key.StartsWith("sk-"))
        {
            try
            {
                var userDto = jwtHelper.GetUserFromToken(key);

                if (userDto == null)
                {
                    context.Response.StatusCode = 401;
                    throw new UnauthorizedAccessException();
                }

                user = await userService.GetAsync(userDto.Id).ConfigureAwait(false);
                token = null;
            }
            catch (Exception e)
            {
                logger.LogError(e, "解析Token 失败");
                context.Response.StatusCode = 401;
                throw new UnauthorizedAccessException();
            }
        }
        else
        {
            token = await DbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key && x.Disabled == false);

            if (token == null)
            {
                logger.LogWarning("Token 不存在");
                context.Response.StatusCode = 401;
                throw new UnauthorizedAccessException();
            }

            // token过期
            if (token.ExpiredTime < DateTimeOffset.Now)
            {
                logger.LogWarning("Token 过期");
                context.Response.StatusCode = 401;
                throw new UnauthorizedAccessException();
            }

            // 余额不足
            if (token is { UnlimitedQuota: false } && token.RemainQuota < requestQuota)
            {
                logger.LogWarning("Token 额度不足");
                context.Response.StatusCode = 402;
                throw new InsufficientQuotaException("当前 Token 额度不足，请充值 Token 额度");
            }

            user = await userService.GetAsync(token.Creator).ConfigureAwait(false);
        }

        if (user == null)
        {
            logger.LogWarning("用户不存在");
            context.Response.StatusCode = 401;
            throw new UnauthorizedAccessException();
        }

        if (user.IsDisabled)
        {
            logger.LogWarning("用户已禁用");
            context.Response.StatusCode = 401;
            throw new UnauthorizedAccessException("账号已禁用");
        }

        if (value.QuotaType == ModelQuotaType.ByCount)
        {
            if (user.ResidualCredit < value.PromptRate)
            {
                logger.LogWarning("用户额度不足");
                context.Response.StatusCode = 402;
                throw new InsufficientQuotaException("额度不足");
            }
            else
            {
                return (token, user);
            }
        }
        else if ((user.ResidualCredit >= value.PromptRate) || user.ResidualCredit >= requestQuota)
        {
            return (token, user);
        }

        logger.LogWarning("用户额度不足");
        context.Response.StatusCode = 402;
        throw new InsufficientQuotaException("额度不足");
    }
}