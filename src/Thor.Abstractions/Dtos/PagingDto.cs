namespace Thor.Service.Dto;

public sealed class PagingDto<T>(long total, List<T> items)
{
    public long Total { get; set; } = total;

    public List<T> Items { get; set; } = items;

    public PagingDto() : this(0, new List<T>())
    {
    }
}