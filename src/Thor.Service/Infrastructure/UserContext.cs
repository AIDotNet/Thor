using System.Security.Claims;

namespace Thor.Service.Infrastructure;

[Registration(typeof(IUserContext))]
public sealed class DefaultUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext, IScopeDependency
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