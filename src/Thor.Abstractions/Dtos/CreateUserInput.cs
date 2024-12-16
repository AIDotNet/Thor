namespace Thor.Service.Dto;

public class CreateUserInput
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    /// <summary>
    /// 邀请人id
    /// </summary>
    public string? ShareId { get; set; }
    
    /// <summary>
    /// 如果使用了邮箱验证码注册，需要填写验证码
    /// </summary>
    public string Code { get; set; } = null!;
}