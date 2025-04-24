using System.Security.Claims;
using System.Text.Json;

namespace Thor.Service.Infrastructure;

public sealed class DefaultUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
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

    public string[] Groups
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            var groups = user?.FindFirst(ClaimTypes.GroupSid)?.Value;

            if (string.IsNullOrWhiteSpace(groups))
            {
                return [];
            }

            return JsonSerializer.Deserialize<string[]>(groups) ?? [];
        }
    }
}