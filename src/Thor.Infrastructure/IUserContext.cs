namespace Thor.Service.Infrastructure;

public interface IUserContext
{
    string CurrentUserId { get; }

    string CurrentUserName { get; }

    bool IsAuthenticated { get; }

    bool IsAdmin { get; }
}