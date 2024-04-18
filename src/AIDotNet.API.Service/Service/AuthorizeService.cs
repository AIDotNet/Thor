using AIDotNet.API.Service.Infrastructure;
using AIDotNet.API.Service.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AIDotNet.API.Service.Service;

public sealed class AuthorizeService(IServiceProvider serviceProvider, IMemoryCache memoryCache)
    : ApplicationService(serviceProvider)
{
    public async ValueTask<object> TokenAsync(string account, string pass)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == account || x.Email == account);

        if (user == null)
        {
            throw new Exception("Account does not exist");
        }

        if (user.IsDisabled)
        {
            throw new Exception("Account is disabled");
        }

        if (user.Password != StringHelper.HashPassword(pass, user.PasswordHas))
        {
            throw new Exception("Password error");
        }

        var key = "su-" + StringHelper.GenerateRandomString(38);

        memoryCache.Set(key, user, TimeSpan.FromDays(3));

        return new
        {
            token = key,
            role = user.Role
        };
    }
}