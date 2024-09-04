namespace Thor.RabbitMQEvent;

public class EventEto
{
    public string FullName { get; set; }

    public byte[] Data { get; set; }

    public EventEto(string fullName, byte[] data)
    {
        FullName = fullName;
        Data = data;
    }
}