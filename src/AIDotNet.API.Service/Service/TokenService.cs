using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Exceptions;
using AIDotNet.API.Service.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public sealed class TokenService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    public async Task CreateAsync(TokenInput input)
    {
        if (input.ExpiredTime < DateTime.Now)
        {
            throw new Exception("过期时间不能小于当前时间");
        }

        if (input is { UnlimitedExpired: false, ExpiredTime: null })
        {
            throw new Exception("请选择过期时间");
        }

        var token = Mapper.Map<Token>(input);

        token.Key = "sk-" + StringHelper.GenerateRandomString(38);

        await DbContext.Tokens.AddAsync(token);

        await DbContext.SaveChangesAsync();
    }

    public async Task<Token?> GetAsync(long id)
    {
        return await DbContext.Tokens
            .FindAsync(id);
    }

    public async Task<PagingDto<Token>> GetListAsync(int page, int pageSize, string? token, string? keyword)
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

    public async Task<bool> UpdateAsync(Token input)
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

    public async Task<bool> RemoveAsync(long id)
    {
        var result = await DbContext.Tokens.Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task DisableAsync(long id)
    {
        var token = await DbContext.Tokens.FindAsync(id);
        if (token == null)
        {
            throw new Exception("Token不存在");
        }

        token.Disabled = true;

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// 校验Token 是否有效
    /// 检验账号额度是否足够
    /// </summary>
    /// <param Name="context"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="InsufficientQuotaException"></exception>
    public async Task<(Token, User)> CheckTokenAsync(HttpContext context)
    {
        var key = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "").Trim();

        var token = await DbContext.Tokens.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key);

        if (token == null)
        {
            context.Response.StatusCode = 401;
            throw new UnauthorizedAccessException();
        }

        // token过期
        if (token.ExpiredTime < DateTimeOffset.Now)
        {
            context.Response.StatusCode = 401;
            throw new UnauthorizedAccessException();
        }

        // 余额不足
        if (token is { UnlimitedQuota: false, RemainQuota: < 0 })
        {
            context.Response.StatusCode = 402;
            throw new InsufficientQuotaException("额度不足");
        }


        var user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == token.Creator);

        if (user == null)
        {
            context.Response.StatusCode = 401;
            throw new UnauthorizedAccessException();
        }

        // 判断额度是否足够
        if (user.ResidualCredit < 10000)
        {
            context.Response.StatusCode = 402;
            throw new InsufficientQuotaException("额度不足");
        }

        return (token, user);
    }
}