namespace Thor.Service.Dto;

public class UseModelDto
{
    public string Model { get; set; }

    /// <summary>
    /// 是否热门
    /// </summary>
    public bool Hot  { get; set; }

    /// <summary>
    /// 使用次数
    /// </summary>
    public long Count { get; set; }
}