using AIDotNet.API.Service.Infrastructure.Helper;
using AIDotNet.API.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public sealed class AuthorizeService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    public async Task<string> TokenAsync(string account, string pass)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == account);

        if (user == null)
        {
            throw new Exception("Account does not exist");
        }

        if (user.Password != StringHelper.HashPassword(pass, user.PasswordHas))
        {
            throw new Exception("Password error");
        }

        return JwtHelper.GeneratorAccessToken(user);
    }
}