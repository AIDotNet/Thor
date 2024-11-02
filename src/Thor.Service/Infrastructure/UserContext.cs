using System.Security.Claims;

namespace Thor.Service.Infrastructure;

[Registration(typeof(IUserContext))]
public sealed class DefaultUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext, IScopeDependency
{
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