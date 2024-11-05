namespace Thor.Rabbit;

public class RabbitOptions
{
    public string ConnectionString { get; set; }

    public List<ConsumeOptions> Consumes { get; set; } = new();
}

public class ConsumeOptions
{
    public string Queue { get; set; }

    public bool AutoAck { get; set; }

    public ushort FetchCount { get; set; }

    /// <summary>
    /// handle发生异常，是否重回队列
    /// </summary>
    public bool FailedRequeue { get; set; }

    public Action<IDeclaration> Declaration { get; set; }
}