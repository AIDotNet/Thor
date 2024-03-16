using AIDotNet.API.Service.Domina.Core;

namespace TokenApi.Contract.Domain;

public sealed class ChatChannel : Entity<string>
{
    /// <summary>
    /// 优先级
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// AI类型
    /// </summary>
    public string Type { get; set; } 
    
    public string Name { get; set; }

    /// <summary>
    /// 根地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? ResponseTime { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 模型
    /// </summary>
    public List<string> Models { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Other { get; set; }
}