namespace AIDotNet.API.Service.Dto;

public sealed class ChatChannelInput
{
    public string Name { get; set; }

    /// <summary>
    /// 根地址
    /// </summary>
    public string Address { get; set; }

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
    
    /// <summary>
    /// AI类型
    /// </summary>
    public string Type { get; set; } 
}