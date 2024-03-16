using AIDotNet.API.Service.Infrastructure.Helper;
using AIDotNet.API.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace AIDotNet.API.Service.Service;

public sealed class AuthorizeService(IServiceProvider serviceProvider) : ApplicationService(serviceProvider)
{
    public async Task<ResultDto> TokenAsync(string account, string pass)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == account);

        if (user == null)
        {
            return ResultDto.CreateFail("Account does not exist");
        }

        if (user.Password != StringHelper.HashPassword(pass, user.PasswordHas))
        {
            return ResultDto.CreateFail("Password error");
        }

        return ResultDto.CreateSuccess("Login successful", JwtHelper.GeneratorAccessToken(user));
    }
}