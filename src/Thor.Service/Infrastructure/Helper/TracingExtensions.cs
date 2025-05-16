using System.Diagnostics;
using Thor.Domain.Chats;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 链路跟踪扩展方法
/// </summary>
public static class TracingExtensions
{
    /// <summary>
    /// 创建一个跟踪范围，在完成时自动结束
    /// </summary>
    /// <param name="name">节点名称</param>
    /// <param name="type">节点类型</param>
    /// <returns>IDisposable对象，用于在using语句中自动结束跟踪</returns>
    public static IDisposable CreateTracingScope(string name, int type = 0)
    {
        return new TracingScope(name, type);
    }

    /// <summary>
    /// 获取当前的根跟踪
    /// </summary>
    /// <returns>根跟踪实体</returns>
    public static Tracing? GetCurrentRootTracing()
    {
        return TracingService.CurrentRootTracing;
    }

    /// <summary>
    /// 记录标签信息到当前Activity
    /// </summary>
    /// <param name="key">标签键</param>
    /// <param name="value">标签值</param>
    public static void AddTag(string key, string value)
    {
        Activity.Current?.SetTag(key, value);
    }

    /// <summary>
    /// 记录事件到当前Activity
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="tags">事件标签</param>
    public static void AddEvent(string name, Dictionary<string, object>? tags = null)
    {
        if (Activity.Current != null)
        {
            var activityEvent = new ActivityEvent(name, tags: new ActivityTagsCollection(
                tags?.Select(kv => new KeyValuePair<string, object?>(kv.Key, kv.Value)) 
                ?? Enumerable.Empty<KeyValuePair<string, object?>>()));
            
            Activity.Current.AddEvent(activityEvent);
        }
    }

    /// <summary>
    /// 跟踪范围，用于自动创建和结束跟踪
    /// </summary>
    private class TracingScope : IDisposable
    {
        private readonly Tracing _tracing;
        private bool _disposed;

        public TracingScope(string name, int type)
        {
            _tracing = TracingService.CreateTracing(name, type);
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            TracingService.EndTracing(_tracing);
            _disposed = true;
        }
    }
} 