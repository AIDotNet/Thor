namespace Thor.Service.Domain.Core;

public abstract class Entity<TKey> : ICreatable, IUpdatable
{
    public TKey Id { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? Modifier { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Creator { get; set; }
}

public interface ICreatable
{
    public DateTime CreatedAt { get; set; }

    public string? Creator { get; set; }
}

public interface IUpdatable
{
    public DateTime? UpdatedAt { get; set; }

    public string? Modifier { get; set; }
}