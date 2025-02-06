using Thor.Abstractions.Realtime;

namespace Thor.SiliconFlow.Realtime;

public class SiliconFlowRealtimeService :IThorRealtimeService
{
    public IRealtimeClient CreateClient()
    {
        return new SiliconFlowRealtimeClient();
    }
}