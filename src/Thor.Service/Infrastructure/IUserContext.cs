using System.Security.Claims;
using Thor.Service.Dto;

namespace Thor.Service.Infrastructure;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    string CurrentUserId { get; }

    string CurrentUserName { get; }

    bool IsAuthenticated { get; }

    bool IsAdmin { get; }
}

public sealed class DefaultUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserDto CurrentUser
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                return null;
            }

            var userId = user.FindFirst(ClaimTypes.Sid)?.Value;
            var userName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return new UserDto()
            {
                UserName = userName,
                Id = userId
            };
        }
    }

    public string CurrentUserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.Sid)?.Value;
        }
    }

    public string CurrentUserName
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.Identity?.IsAuthenticated ?? false;
        }
    }

    public bool IsAdmin
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user?.IsInRole(RoleConstant.Admin) ?? false;
        }
    }
}