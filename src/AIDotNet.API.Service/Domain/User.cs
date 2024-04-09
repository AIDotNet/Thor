using AIDotNet.API.Service.Domain.Core;
using AIDotNet.API.Service.Domina.Core;
using AIDotNet.API.Service.Infrastructure.Helper;

namespace AIDotNet.API.Service.Domain;

public sealed class User : Entity<string>, ISoftDeletion
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordHas { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    public string Role { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// 消耗的Token
    /// </summary>
    public long ConsumeToken { get; set; }

    /// <summary>
    /// 请求总数
    /// </summary>
    public long RequestCount { get; set; }

    /// <summary>
    /// 账号额度
    /// </summary>
    public long ResidualCredit { get; set; }
    
    protected User()
    {
    }

    public User(string id, string userName, string email, string password)
    {
        Id = id;
        UserName = userName;
        Email = email;
        SetUser();
        SetPassword(password);
    }

    public void SetAdmin()
    {
        Role = RoleConstant.Admin;
    }

    public void SetUser()
    {
        Role = RoleConstant.User;
    }


    public void SetPassword(string password)
    {
        PasswordHas = Guid.NewGuid().ToString("N");
        Password = StringHelper.HashPassword(password, PasswordHas);
    }
    
    public void SetResidualCredit(long residualCredit)
    {
        ResidualCredit = residualCredit;
    }
}