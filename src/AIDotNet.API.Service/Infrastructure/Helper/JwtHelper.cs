using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AIDotNet.API.Service.Dto;
using AIDotNet.API.Service.Options;
using Microsoft.IdentityModel.Tokens;
using TokenApi.Contract.Domain;

namespace AIDotNet.API.Service.Infrastructure.Helper;

public static class JwtHelper
{
    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claimsIdentity"></param>
    /// <returns></returns>
    public static string GeneratorAccessToken(ClaimsIdentity claimsIdentity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtOptions.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(JwtOptions.EffectiveHours),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claimsIdentity"></param>
    /// <returns></returns>
    public static string GeneratorAccessToken(User user)
    {
        var claimsIdentity = GetClaimsIdentity(user);

        return GeneratorAccessToken(claimsIdentity);
    }


    public static ClaimsIdentity GetClaimsIdentity(User user)
    {
        return new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.UserName),
            new(ClaimTypes.Sid, user.Id),
        });
    }

    public static UserDto GetCurrentUser(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.FindFirstValue(ClaimTypes.Sid) is null)
            return null;
        var user = new UserDto
        {
            UserName = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
            Id = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value
        };

        return user;
    }

    public static UserDto? GetCurrentUser(string token)
    {
        if (token == null)
        {
            return null;
        }

        try
        {
            // 使用jwt解析token
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var user = new UserDto
            {
                UserName = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value,
                Id = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            };
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }
}