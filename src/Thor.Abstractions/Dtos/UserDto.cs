namespace Thor.Service.Dto;

public class UserDto
{
    public string Id { get; set; }
    
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;
}