namespace Thor.Service.Dto;

public sealed class ChatChannelInput
{
    public string Name { get; set; }

    /// <summary>
    /// 根地址
    /// </summary>
    public string Address { get; set; } = string.Empty;

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
    public string Other { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段
    /// </summary>
    public Dictionary<string, string> Extension { get; set; } = new();
    
    /// <summary>
    /// AI类型
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// 分组
    /// </summary>
    /// <returns></returns>
    public string[] Groups { get; set; } = [];
    
    /// <summary>
    /// 是否支持Responses
    /// </summary>
    /// <returns></returns>
    public bool SupportsResponses { get; set; } = false;
}