namespace Thor.Service.Domain.Core;

/// <summary>
/// 产品订单状态
/// </summary>
public enum ProductPurchaseStatus
{
    /// <summary>
    /// 未支付
    /// </summary>
    Unpaid = 0,

    /// <summary>
    /// 已支付
    /// </summary>
    Paid = 1,

    /// <summary>
    /// 已取消
    /// </summary>
    Canceled = 2,

    /// <summary>
    /// 已退款
    /// </summary>
    Refunded = 3,
}