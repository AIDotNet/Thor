namespace Thor.Service.Infrastructure;

public class ChannelAsyncLocal
{
    private static readonly AsyncLocal<ChannelHolder> _channelHolder = new();

    public static List<string> ChannelIds
    {
        get => _channelHolder.Value?.ChannelIds ?? new List<string>(3);
        set
        {
            _channelHolder.Value ??= new ChannelHolder();

            _channelHolder.Value.ChannelIds = value;
        }
    }

    private sealed class ChannelHolder
    {
        /// <summary>
        /// 已经试用的渠道ID
        /// </summary>
        public List<string> ChannelIds { get; set; } = new(3);
    }
}