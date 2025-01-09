namespace Thor.Abstractions.Dtos;

public class TrackerDto
{
    /// <summary>
    /// 服务状态信息
    /// </summary>
    public string Tooltip { get; set; }
    
    /// <summary>
    /// 服务状态 百分比
    /// </summary>
    /// <returns></returns>
    public int Percentage { get; set; }

    /// <summary>
    /// 状态颜色
    /// </summary>
    public string Color { get; set; }
    
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }
}