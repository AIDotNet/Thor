namespace Thor.Service.Options;

public class JwtOptions
{
    public const string Name = "Jwt";
    
    /// <summary>
    /// Կ
    /// </summary>
    public static string Secret { get; set; }

    /// <summary>
    /// Ч
    /// </summary>
    public static int EffectiveHours { get; set; }
}