using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

/// <summary>
/// 模型映射
/// </summary>
public sealed class ModelMap : Entity<Guid>
{
    /// <summary>
    /// 匹配模型id
    /// </summary>
    public string ModelId { get; set; }

    /// <summary>
    /// 映射模型
    /// </summary>
    public List<ModelMapItem> ModelMapItems { get; set; } = new();

    /// <summary>
    /// 生效分组
    /// </summary>
    public string[] Group { get; set; } = [];
}

public sealed class ModelMapItem
{
    /// <summary>
    /// 映射模型id
    /// </summary>
    public string ModelId { get; set; }

    /// <summary>
    /// 模型权重 
    /// </summary>
    public int Order { get; set; }
}