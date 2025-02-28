using Thor.Abstractions;
using Thor.Service.Domain.Core;
using Thor.Service.Infrastructure.Helper;

namespace Thor.Service.Domain;

public sealed class User : Entity<string>, ISoftDeletion
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordHas { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisabled { get; set; }

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
    /// 分组
    /// </summary>
    /// <returns></returns>
    public string[] Groups { get; set; }

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
        IsDisabled = false;
        IsDelete = false;
        DeletedAt = null;
        ConsumeToken = 0;
        RequestCount = 0;
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

    public bool VerifyPassword(string password)
    {
        return StringHelper.HashPassword(password, PasswordHas) == Password;
    }
}