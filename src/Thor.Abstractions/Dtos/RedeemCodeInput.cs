namespace Thor.Service.Dto;

public class RedeemCodeInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 额度
    /// </summary>
    public long Quota { get; set; }

    /// <summary>
    /// 生成数量
    /// </summary>
    public int Count { get; set; }
}