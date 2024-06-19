using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

/// <summary>
/// 产品类
/// </summary>
public class Product : Entity<string>
{
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 产品价格
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// 额度
    /// </summary>
    public long RemainQuota { get; set; }
    
    /// <summary>
    /// 产品库存 如果为-1则表示无限库存
    /// </summary>
    public int Stock { get; set; }
}