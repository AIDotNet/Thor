namespace Thor.Service.Domain.Core;

public interface ISoftDeletion
{
    public bool IsDelete { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}
