using AIDotNet.API.Service.Domina;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure.Helper;
using AIDotNet.API.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public sealed class TokenService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    public async Task CreateAsync(TokenInput input)
    {
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

    public async Task<PagingDto<Token>> GetListAsync(int page, int size)
    {
        var total = await DbContext.Tokens
            .Where(x => x.Creator == UserContext.CurrentUserId)
            .CountAsync();

        var items = await DbContext.Tokens
            .Where(x => x.Creator == UserContext.CurrentUserId)
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagingDto<Token>(total, items);
    }
    
    public async Task<ResultDto<bool>> UpdateAsync(Token input)
    {
        var result = await DbContext.Tokens.Where(x=>x.Id==input.Id)
            .ExecuteUpdateAsync(item =>
                item.SetProperty(x => x.Name, input.Name)
                    .SetProperty(x => x.ExpiredTime, input.ExpiredTime)
                    .SetProperty(x => x.RemainQuota, input.RemainQuota)
                    .SetProperty(x => x.UnlimitedQuota, input.UnlimitedQuota)
            );
        
        return ResultDto<bool>.CreateSuccess(result > 0);
    }
    
    public async Task<ResultDto<bool>> RemoveAsync(long id)
    {
        var result = await DbContext.Tokens.Where(x=>x.Id==id)
            .ExecuteDeleteAsync();
        
        return ResultDto<bool>.CreateSuccess(result > 0);
    }
}