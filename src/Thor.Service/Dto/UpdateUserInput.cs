namespace Thor.Service.Dto;

public class UpdateUserInput
{
    public string Email { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }
    
}