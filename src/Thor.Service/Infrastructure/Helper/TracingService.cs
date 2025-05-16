using System.Diagnostics;
using System.Collections.Concurrent;
using Thor.Domain.Chats;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 链路跟踪服务
/// </summary>
public class TracingService
{
    /// <summary>
    /// 存储当前线程中的所有Tracing
    /// </summary>
    private class TracingList
    {
        public List<Tracing> Tracings { get; } = new List<Tracing>();
    }

    private static readonly AsyncLocal<TracingList> _tracingList = new();


    private static readonly ConcurrentDictionary<string, Tracing> _activityTracingMap = new();
    private static readonly ConcurrentDictionary<string, Tracing> _traceIdRootMap = new();

    // 确保 DiagnosticListener 被创建并已订阅，以便能够监听 Activity
    private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener("Thor.Diagnostic");

    /// <summary>
    /// 获取当前线程中的所有跟踪
    /// </summary>
    public static List<Tracing> CurrentTracings => _tracingList.Value?.Tracings ?? new List<Tracing>();

    /// <summary>
    /// 获取当前活动相关的根跟踪（向后兼容）
    /// </summary>
    public static Tracing? CurrentRootTracing
    {
        get
        {
            var current = Activity.Current;

            // 当前有活动，尝试通过TraceId找根节点
            if (current != null && _traceIdRootMap.TryGetValue(current.TraceId.ToString(), out var rootByTraceId))
            {
                return rootByTraceId;
            }

            // 找不到，返回第一个跟踪节点（如果存在）
            return CurrentTracings.FirstOrDefault();
        }
    }

    static TracingService()
    {
        // 确保 Activity 被正确监听
        DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());

        // 监听活动变更事件
        Activity.CurrentChanged += OnActivityChanged;
    }

    /// <summary>
    /// 向当前线程添加跟踪对象
    /// </summary>
    public static void AddTracingToCurrentThread(Tracing tracing)
    {
        if (_tracingList.Value == null)
        {
            _tracingList.Value = new TracingList();
        }

        if (!_tracingList.Value.Tracings.Contains(tracing))
        {
            _tracingList.Value.Tracings.Add(tracing);
        }

        // 同时更新TraceId到根节点的映射
        if (!string.IsNullOrEmpty(tracing.TraceId))
        {
            _traceIdRootMap[tracing.TraceId] = tracing;
        }
    }

    /// <summary>
    /// 强制设置根跟踪节点（用于手动确保根节点存在，保持向后兼容）
    /// </summary>
    public static void SetRootTracing(Tracing tracing)
    {
        // 清除之前的跟踪列表
        _tracingList.Value = new TracingList();

        // 添加为第一个根跟踪
        AddTracingToCurrentThread(tracing);
    }

    private static void OnActivityChanged(object? sender, ActivityChangedEventArgs e)
    {
        var current = e.Current;

        // 当前活动为空时，忽略
        if (current == null) return;

        if (!_activityTracingMap.TryGetValue(current.Id ?? string.Empty, out var existingTracing))
        {
            // 新活动，创建跟踪
            ProcessNewActivity(current);
        }
        else
        {
            // 更新现有活动信息
            // Activity.Stopped属性表明活动是否已结束
            if (current.Duration != TimeSpan.Zero || current.Status == ActivityStatusCode.Error)
            {
                UpdateEndedActivity(current);
            }
        }
    }

    private static void ProcessNewActivity(Activity activity)
    {
        var traceId = activity.TraceId.ToString();
        var tracing = new Tracing
        {
            Id = Guid.NewGuid().ToString("N"),
            TraceId = traceId,
            Name = activity.DisplayName,
            StartTime = activity.StartTimeUtc == default ? DateTime.UtcNow : activity.StartTimeUtc,
            ServiceName = activity.Source.Name,
            Attributes = new Dictionary<string, string>(),
            Status = 1,
            Type = 0,
        };

        if (activity.Parent != null)
        {
            // 获取父节点
            string? p = activity.ParentId;
            if (!string.IsNullOrEmpty(p) && _activityTracingMap.TryGetValue(p, out var parentTracing))
            {
                // 尝试找到同级别的上一个Tracing
                Tracing? lastSiblingTracing = parentTracing.Children.LastOrDefault();

                if (lastSiblingTracing != null && lastSiblingTracing != tracing)
                {
                    // 存在同级节点，使用同级节点的开始时间
                    tracing.Duration = (long)(activity.StartTimeUtc - lastSiblingTracing.StartTime).TotalMilliseconds;
                }
                else
                {
                    // 不存在同级节点，使用父节点的开始时间
                    tracing.Duration = (long)(activity.StartTimeUtc - parentTracing.StartTime).TotalMilliseconds;
                }
            }
            else
            {
                // 找不到父节点，使用父Activity的开始时间
                tracing.Duration = activity.Parent.StartTimeUtc == default
                    ? 0
                    : (long)(activity.StartTimeUtc - activity.Parent.StartTimeUtc).TotalMilliseconds;
            }
        }
        else
        {
            tracing.Duration = 0; // 根节点没有时间差
        }

        // 添加基本属性
        if (activity.Source.Tags != null)
        {
            foreach (var tag in activity.Source.Tags)
            {
                tracing.Attributes[tag.Key] = tag.Value?.ToString() ?? string.Empty;
            }
        }

        // 添加标签作为属性
        if (activity.TagObjects.Any())
        {
            tracing.Attributes ??= new Dictionary<string, string>();
            foreach (var tag in activity.TagObjects)
            {
                tracing.Attributes[tag.Key] = tag.Value?.ToString() ?? string.Empty;
            }
        }

        // 先保存到映射表，方便后续引用
        _activityTracingMap[activity.Id ?? string.Empty] = tracing;

        // 处理父子关系
        string? parentId = activity.ParentId;
        bool isRoot = string.IsNullOrEmpty(parentId);

        if (!isRoot)
        {
            // 尝试找到父Tracing
            Tracing? parentTracing = null;

            // 1. 通过父Activity ID查找
            if (!string.IsNullOrEmpty(parentId))
            {
                _activityTracingMap.TryGetValue(parentId, out parentTracing);
            }

            // 2. 通过TraceId查找根Tracing
            if (parentTracing == null && _traceIdRootMap.TryGetValue(traceId, out var rootTracing))
            {
                // 找到了同TraceId的根节点，尝试遍历查找父节点
                parentTracing = FindTracingByName(rootTracing, activity.Parent?.DisplayName);
            }

            // 3. 通过当前根Tracing列表查找
            if (parentTracing == null)
            {
                foreach (var root in CurrentTracings)
                {
                    if (root.TraceId == traceId)
                    {
                        parentTracing = FindTracingByName(root, activity.Parent?.DisplayName);
                        if (parentTracing != null) break;
                    }
                }
            }

            if (parentTracing != null)
            {
                // 找到父Tracing，建立关联
                parentTracing.Children.Add(tracing);
                tracing.Depth = parentTracing.Depth + 1;

            }
            else
            {
                // 未找到父Tracing，但有父Activity，可能是根节点未正确设置
                // 此时将当前Tracing设为根，并关联到TraceId
                tracing.Depth = 0;

                if (!_traceIdRootMap.ContainsKey(traceId))
                {
                    _traceIdRootMap[traceId] = tracing;
                }

                // 添加到当前线程的根跟踪列表
                AddTracingToCurrentThread(tracing);
            }
        }
        else
        {
            // 这是一个根Activity
            tracing.Depth = 0;

            // 更新根映射
            _traceIdRootMap[traceId] = tracing;

            // 添加到当前线程的根跟踪列表
            AddTracingToCurrentThread(tracing);
        }
    }

    private static void UpdateEndedActivity(Activity activity)
    {
        if (_activityTracingMap.TryGetValue(activity.Id ?? string.Empty, out var tracing))
        {
            tracing.EndTime = activity.StartTimeUtc + activity.Duration;
            tracing.Duration = (long)activity.Duration.TotalMilliseconds;

            // 更新标签作为属性
            if (activity.TagObjects.Any())
            {
                tracing.Attributes ??= new Dictionary<string, string>();
                foreach (var tag in activity.TagObjects)
                {
                    tracing.Attributes[tag.Key] = tag.Value?.ToString() ?? string.Empty;
                }
            }
        }
    }

    private static Tracing? FindTracingByName(Tracing root, string? name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        if (root.Name == name)
            return root;

        foreach (var child in root.Children)
        {
            var result = FindTracingByName(child, name);
            if (result != null)
                return result;
        }

        return null;
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
        var traceId = activity?.TraceId.ToString() ?? Guid.NewGuid().ToString();

        var tracing = new Tracing
        {
            Id = Guid.NewGuid().ToString("N"),
            TraceId = traceId,
            Name = name,
            Type = type,
            StartTime = DateTime.UtcNow,
            ServiceName = activity?.Source.Name ?? "Manual",
            Status = 0,
            Depth = 0
        };

        if (activity != null)
        {
            // 如果当前有活动，尝试添加到现有链路
            var parentTracing = GetTracingByActivityId(activity.Id);
            if (parentTracing != null)
            {
                tracing.Depth = parentTracing.Depth + 1;
                parentTracing.Children.Add(tracing);
            }
            else if (_traceIdRootMap.TryGetValue(traceId, out var rootByTraceId))
            {
                // 通过TraceId找到了根节点
                tracing.Depth = 1;
                rootByTraceId.Children.Add(tracing);
            }
            else
            {
                // 检查当前线程是否有跟踪列表
                var rootTracings = CurrentTracings;
                if (rootTracings.Any())
                {
                    // 关联到第一个根节点
                    var root = rootTracings.First();
                    tracing.Depth = root.Depth + 1;
                    root.Children.Add(tracing);
                }
                else
                {
                    // 无法找到任何父节点，设为根节点
                    _traceIdRootMap[traceId] = tracing;
                    AddTracingToCurrentThread(tracing);
                }
            }
        }
        else if (CurrentTracings.Count == 0)
        {
            // 如果没有根链路，设置为根
            _traceIdRootMap[traceId] = tracing;
            AddTracingToCurrentThread(tracing);
        }
        else
        {
            // 添加到现有根链路列表的第一个
            var root = CurrentTracings.First();
            tracing.Depth = root.Depth + 1;
            root.Children.Add(tracing);
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
    /// 清除当前线程的所有跟踪
    /// </summary>
    public static void ClearCurrentTracing()
    {
        // 清空当前线程的Tracing列表
        if (_tracingList.Value != null)
        {
            _tracingList.Value.Tracings.Clear();
        }

        // 清除TraceId相关的内容
        var currentActivity = Activity.Current;
        if (currentActivity != null)
        {
            var traceId = currentActivity.TraceId.ToString();
            _traceIdRootMap.TryRemove(traceId, out _);

            // 清除所有相同TraceId的Activity映射
            var keysToRemove = _activityTracingMap.Where(kvp =>
                GetTracingByActivityId(kvp.Key)?.TraceId == traceId
            ).Select(kvp => kvp.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _activityTracingMap.TryRemove(key, out _);
            }
        }
        else
        {
            // 无法通过当前Activity确定TraceId，清除全部
            _activityTracingMap.Clear();
            _traceIdRootMap.Clear();
        }
    }

    /// <summary>
    /// 用于订阅所有诊断监听器的观察者
    /// </summary>
    private class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

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
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object?> value)
        {
            // 可以在这里处理特定的诊断事件
            // Console.WriteLine($"Diagnostic event: {value.Key}");
        }
    }
}