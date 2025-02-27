namespace Thor.Abstractions.Dtos;

public class AdminUpdateInput
{
    public string Id { get; set ;}

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    /// <summary>
    /// Groups
    /// </summary>
    public string[] Groups { get; set; } = [];
}