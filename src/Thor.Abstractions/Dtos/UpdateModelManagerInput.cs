namespace Thor.Service.Dto;

public class UpdateModelManagerInput : CreateModelManagerInput
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }
}