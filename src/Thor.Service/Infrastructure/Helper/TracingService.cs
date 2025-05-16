using System.Diagnostics;
using System.Collections.Concurrent;
using Thor.Domain.Chats;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 链路跟踪服务
/// </summary>
public class TracingService
{
    private static readonly AsyncLocal<Tracing?> _rootTracing = new();
    private static readonly ConcurrentDictionary<string, Tracing> _activityTracingMap = new();
    
    // 确保 DiagnosticListener 被创建并已订阅，以便能够监听 Activity
    private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener("Thor.Diagnostic");
    
    /// <summary>
    /// 获取当前顶层链路
    /// </summary>
    public static Tracing? CurrentRootTracing => _rootTracing.Value;

    static TracingService()
    {
        // 确保 Activity 被正确监听
        DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());
        
        // 监听活动变更事件
        Activity.CurrentChanged += OnActivityChanged;
    }

    /// <summary>
    /// 强制设置根跟踪节点（用于手动确保根节点存在）
    /// </summary>
    /// <param name="tracing">要设置为根节点的跟踪对象</param>
    public static void SetRootTracing(Tracing tracing)
    {
        _rootTracing.Value = tracing;
    }

    private static void OnActivityChanged(object? sender, ActivityChangedEventArgs e)
    {
        var current = e.Current;
        
        // 当前活动为空时，忽略
        if (current == null) return;

        // 确保每次变更都会记录到控制台，帮助排查问题
        Console.WriteLine($"Activity changed: {current.DisplayName}, ID: {current.Id}, Parent: {current.ParentId}");

        // 如果是新创建的活动
        if (!_activityTracingMap.ContainsKey(current.Id ?? string.Empty))
        {
            var tracing = new Tracing
            {
                Id = Guid.NewGuid().ToString("N"),
                TraceId = current.TraceId.ToString(),
                Name = current.DisplayName,
                StartTime = current.StartTimeUtc.Date == default ? DateTime.UtcNow : current.StartTimeUtc,
                ServiceName = current.Source.Name,
                Status = 0, // 默认状态
                Type = 0,   // 默认类型
                Depth = current.Parent != null ? 
                    GetTracingByActivityId(current.Parent.Id)?.Depth + 1 ?? 0 : 0
            };

            // 添加标签作为属性
            if (current.TagObjects.Any())
            {
                tracing.Attributes ??= new Dictionary<string, string>();
                foreach (var tag in current.TagObjects)
                {
                    tracing.Attributes[tag.Key] = tag.Value?.ToString() ?? string.Empty;
                }
            }

            // 添加到活动-跟踪映射
            _activityTracingMap[current.Id ?? string.Empty] = tracing;

            // 如果有父活动，将当前跟踪添加到父跟踪的子集中
            if (current.Parent != null && !string.IsNullOrEmpty(current.Parent.Id))
            {
                var parentTracing = GetTracingByActivityId(current.Parent.Id);
                if (parentTracing != null)
                {
                    parentTracing.Children.Add(tracing);
                }
            }
            else
            {
                // 如果没有父活动，则这是根活动，设置到 AsyncLocal
                _rootTracing.Value = tracing;
                Console.WriteLine($"Root tracing set: {tracing.Name}, ID: {tracing.Id}");
            }
        }
        else if (current.Duration != TimeSpan.Zero) // 活动结束
        {
            // 更新活动信息
            var tracing = GetTracingByActivityId(current.Id);
            if (tracing != null)
            {
                tracing.EndTime = current.StartTimeUtc + current.Duration;
                tracing.Duration = (long)current.Duration.TotalMilliseconds;
                
                // 更新标签作为属性
                if (current.TagObjects.Any())
                {
                    tracing.Attributes ??= new Dictionary<string, string>();
                    foreach (var tag in current.TagObjects)
                    {
                        tracing.Attributes[tag.Key] = tag.Value?.ToString() ?? string.Empty;
                    }
                }
            }
        }
    }

    private static Tracing? GetTracingByActivityId(string? activityId)
    {
        if (string.IsNullOrEmpty(activityId)) return null;
        
        return _activityTracingMap.TryGetValue(activityId, out var tracing) ? tracing : null;
    }

    /// <summary>
    /// 手动创建一个跟踪节点
    /// </summary>
    /// <param name="name">节点名称</param>
    /// <param name="type">节点类型</param>
    /// <returns>创建的跟踪实体</returns>
    public static Tracing CreateTracing(string name, int type = 0)
    {
        var activity = Activity.Current;
        var tracing = new Tracing
        {
            Id = Guid.NewGuid().ToString("N"),
            TraceId = activity?.TraceId.ToString() ?? Guid.NewGuid().ToString(),
            Name = name,
            Type = type,
            StartTime = DateTime.UtcNow,
            ServiceName = activity?.Source.Name ?? "Manual",
            Status = 0,
            Depth = 0
        };

        if (activity != null)
        {
            // 如果当前有活动，添加到现有链路
            var parentTracing = GetTracingByActivityId(activity.Id);
            if (parentTracing != null)
            {
                tracing.Depth = parentTracing.Depth + 1;
                parentTracing.Children.Add(tracing);
            }
        }
        else if (_rootTracing.Value == null)
        {
            // 如果没有根链路，设置为根
            _rootTracing.Value = tracing;
            Console.WriteLine($"Manual root tracing set: {tracing.Name}, ID: {tracing.Id}");
        }
        else
        {
            // 添加到现有根链路
            tracing.Depth = _rootTracing.Value.Depth + 1;
            _rootTracing.Value.Children.Add(tracing);
        }

        return tracing;
    }

    /// <summary>
    /// 结束跟踪节点
    /// </summary>
    /// <param name="tracing">要结束的跟踪实体</param>
    /// <param name="status">状态</param>
    /// <param name="errorMessage">错误信息</param>
    public static void EndTracing(Tracing tracing, int status = 0, string? errorMessage = null)
    {
        tracing.EndTime = DateTime.UtcNow;
        tracing.Duration = (long)(tracing.EndTime.Value - tracing.StartTime).TotalMilliseconds;
        tracing.Status = status;
        tracing.ErrorMessage = errorMessage;
    }

    /// <summary>
    /// 清除当前线程的根链路
    /// </summary>
    public static void ClearCurrentTracing()
    {
        _rootTracing.Value = null;
        _activityTracingMap.Clear();
    }
    
    /// <summary>
    /// 用于订阅所有诊断监听器的观察者
    /// </summary>
    private class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DiagnosticListener listener)
        {
            // 订阅所有Thor相关的诊断监听器
            if (listener.Name.StartsWith("Thor") || listener.Name.Contains("Microsoft.AspNetCore"))
            {
                listener.Subscribe(new KeyValueObserver());
            }
        }
    }

    /// <summary>
    /// 用于观察诊断事件的观察者
    /// </summary>
    private class KeyValueObserver : IObserver<KeyValuePair<string, object?>>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(KeyValuePair<string, object?> value)
        {
            // 可以在这里处理特定的诊断事件
            Console.WriteLine($"Diagnostic event: {value.Key}");
        }
    }
} 