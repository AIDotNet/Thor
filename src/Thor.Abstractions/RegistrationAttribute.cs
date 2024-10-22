namespace Thor.Abstractions;


/// <summary>
/// 指定注册接口
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RegistrationAttribute : Attribute
{
    /// <summary>
    /// 注册类型
    /// </summary>
    public Type Type { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    public RegistrationAttribute(Type type)
    {
        Type = type;
    }

    public RegistrationAttribute()
    {
    }
}