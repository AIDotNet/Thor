using AIDotNet.API.Service.Domain.Core;
using AIDotNet.API.Service.Domina.Core;

namespace AIDotNet.API.Service.Domain;

public sealed class Logger : Entity<long>
{
    public int Type { get; set; }
    
    public string Content { get; set; }

    /// <summary>
    /// 请求Token
    /// </summary>
    public int PromptTokens { get; set; }

    /// <summary>
    /// 完成Token
    /// </summary>
    public int CompletionTokens { get; set; }

    /// <summary>
    /// 消费额度
    /// </summary>
    public int Quota { get; set; }

    /// <summary>
    /// 模型
    /// </summary>
    public string ModelName { get; set; }

    /// <summary>
    /// token名称
    /// </summary>
    public string TokenName { get; set; }
}