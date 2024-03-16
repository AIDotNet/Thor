namespace AIDotNet.API.Service.Domina.Core;

public interface ISoftDeletion
{
    public bool IsDelete { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}
