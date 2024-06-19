using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

public sealed class ProductPurchaseRecord : Entity<string>
{
    /// <summary>
    /// 产品ID
    /// </summary>
    public string ProductId { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 购买数量
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// 额度
    /// </summary>
    public long RemainQuota { get; set; }

    /// <summary>
    /// 购买时间
    /// </summary>
    public DateTime PurchaseTime { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public ProductPurchaseStatus Status { get; set; }
    
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}