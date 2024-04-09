using AIDotNet.API.Service.Domain;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public class UserService(IServiceProvider serviceProvider, IUserContext userContext)
    : ApplicationService(serviceProvider)
{
    public async Task<User> CreateAsync(CreateUserInput input)
    {
        // 判断是否存在
        var exist = await DbContext.Users.AnyAsync(x => x.UserName == input.UserName || x.Email == input.Email);
        if (exist)
        {
            throw new Exception("用户名已存在");
        }

        var user = new User(Guid.NewGuid().ToString(), input.UserName, input.Email, input.Password);
        user.SetPassword(input.Password);
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        if (userContext.CurrentUserId == id)
            throw new Exception("不能删除自己");

        var result = await DbContext.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
        return result > 0;
    }

    public async Task<PagingDto<User>> GetAsync(int page, int pageSize, string? keyword)
    {
        var total = await DbContext.Users.CountAsync(x =>
            string.IsNullOrEmpty(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword));

        if (total > 0)
        {
            var result = await DbContext.Users
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(keyword) || x.UserName.Contains(keyword) || x.Email.Contains(keyword))
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingDto<User>(total, result);
        }

        return new PagingDto<User>(total, new List<User>());
    }

    public async ValueTask<bool> ConsumeAsync(string id, long consume, int consumeToken)
    {
        var result = await DbContext
            .Users
            .Where(x => x.Id == id && x.ResidualCredit >= consume)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.ResidualCredit, y => y.ResidualCredit - consume)
                    .SetProperty(y => y.RequestCount, y => y.RequestCount + 1)
                    .SetProperty(y => y.ConsumeToken, y => y.ConsumeToken + consumeToken));

        return result > 0;
    }
}