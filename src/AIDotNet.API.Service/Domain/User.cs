using AIDotNet.API.Service.Domina.Core;
using AIDotNet.API.Service.Infrastructure.Helper;

namespace TokenApi.Contract.Domain;

public sealed class User : Entity<string>, ISoftDeletion
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordHas { get; set; } = null!;

    public bool IsDelete { get; set; }

    public DateTime? DeletedAt { get; set; }

    protected User()
    {
    }

    public User(string id, string userName, string email, string password)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHas = Guid.NewGuid().ToString("N");
        Password = StringHelper.HashPassword(password, PasswordHas);
    }
}