namespace Thor.Service.Eto;

public class CreateUserEto
{
    public User User { get; set; }

    public CreateUserSource Source { get; set; } = CreateUserSource.System;
}

public enum CreateUserSource
{
    /// <summary>
    /// 系统
    /// </summary>
    System = 0,

    /// <summary>
    /// Github
    /// </summary>
    Github = 1,
    
    /// <summary>
    /// Gitee
    /// </summary>
    Gitee = 2
}