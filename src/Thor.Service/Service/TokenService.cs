using Thor.Service.Options;

namespace Thor.Service.Service;

public sealed class TokenService(
    IServiceProvider serviceProvider,
    IServiceCache memoryCache,
    ILogger<TokenService> logger)
    : ApplicationService(serviceProvider)
{
    public async ValueTask CreateAsync(TokenInput input, string? createId = null)
    {
        if (input.ExpiredTime < DateTime.Now) throw new Exception("过期时间不能小于当前时间");

        if (input is { UnlimitedExpired: false, ExpiredTime: null }) throw new Exception("请选择过期时间");

        var token = Mapper.Map<Token>(input);

        if (!createId.IsNullOrEmpty()) token.Creator = createId;

        token.Id = Guid.NewGuid().ToString("N");

        token.Key = "sk-" + StringHelper.GenerateRandomString(38);

        await DbContext.Tokens.AddAsync(token);
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
        var result = await DbContext.Tokens.Where(x => x.Id == input.Id)
            .ExecuteUpdateAsync(item =>
                item.SetProperty(x => x.Name, input.Name)
                    .SetProperty(x => x.ExpiredTime, input.ExpiredTime)
                    .SetProperty(x => x.RemainQuota, input.RemainQuota)
                    .SetProperty(x => x.UnlimitedQuota, input.UnlimitedQuota)
                    .SetProperty(x => x.UnlimitedExpired, input.UnlimitedExpired)
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

    /// <summary>
    ///     校验Token 是否有效
    ///     检验账号额度是否足够
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="InsufficientQuotaException"></exception>
    public async ValueTask<(Token?, User)> CheckTokenAsync(HttpContext context)
    {
        var key = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();

        var requestQuota = SettingService.GetIntSetting(SettingExtensions.GeneralSetting.RequestQuota);

        if (requestQuota <= 0) requestQuota = 5000;

        User? user;
        Token? token;
        // su-则是用户token
        if (key.StartsWith("su-"))
        {
            user = await memoryCache.GetAsync<User>(key);

            if (user == null)
            {
                context.Response.StatusCode = 401;
                throw new UnauthorizedAccessException();
            }

            user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id);
            token = null;
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

            user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == token.Creator);
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

        // 判断额度是否足够
        if (user.ResidualCredit < requestQuota)
        {
            logger.LogWarning("用户额度不足");
            context.Response.StatusCode = 402;
            throw new InsufficientQuotaException("额度不足");
        }

        return (token, user);
    }
}