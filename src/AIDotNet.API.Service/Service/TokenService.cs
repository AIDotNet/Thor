using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
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
        var total = await DbContext.Tokens
            .AsNoTracking()
            .Where(x => x.Creator == UserContext.CurrentUserId)
            .Where(x => string.IsNullOrEmpty(token) || x.Key == token)
            .Where(x => string.IsNullOrEmpty(keyword) || x.Name.Contains(keyword))
            .CountAsync();

        var items = await DbContext.Tokens
            .Where(x => x.Creator == UserContext.CurrentUserId)
            .AsNoTracking()
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
}