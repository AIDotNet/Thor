namespace Thor.Service.Infrastructure;

public interface IUserContext
{
    UserDto CurrentUser { get; }

    string CurrentUserId { get; }

    string CurrentUserName { get; }

    bool IsAuthenticated { get; }

    bool IsAdmin { get; }
}