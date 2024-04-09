namespace AIDotNet.API.Service.Dto;

public class CreateUserInput
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}