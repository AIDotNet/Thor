using Thor.Service.Domain.Core;

namespace Thor.Service.Domain;

public sealed class RedeemCode: Entity<string>
{
    /// <summary>
    /// 兑换名称
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// 对话码
    /// </summary>
    public string Code { get;private  set; }
    
    /// <summary>
    /// 额度
    /// </summary>
    public long Quota { get;private  set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool Disabled { get;private  set; }
    
    /// <summary>
    /// 兑换时间
    /// </summary>
    public DateTime? RedeemedTime { get;private  set; }

    /// <summary>
    /// 对话用户id
    /// </summary>
    public string? RedeemedUserId { get; set; }

    /// <summary>
    /// 兑换用户名称
    /// </summary>
    public string? RedeemedUserName { get; set; }
    
    /// <summary>
    /// 兑换状态
    /// </summary>
    public RedeemedState State { get;private  set; }
    
    protected RedeemCode()
    {
    }
    
    public RedeemCode(string name, long quota)
    {
        Name = name;
        Code = Guid.NewGuid().ToString("N");
        Quota = quota;
        Disabled = false;
        State = RedeemedState.NotRedeemed;
    }
}