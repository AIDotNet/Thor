using System.Diagnostics;

namespace Thor.Service.Infrastructure.Helper;

/// <summary>
/// 活动跟踪设置
/// </summary>
public static class ActivitySetup
{
    /// <summary>
    /// 设置活动监听器
    /// </summary>
    public static void SetupActivityListeners()
    {
        // 确保活动跟踪启用
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
        
        // 全局设置诊断监听器
        DiagnosticListener.AllListeners.Subscribe(new AllDiagnosticListener());
        
        // 启用所有 Thor 相关的活动源
        ActivitySource.AddActivityListener(new ActivityListener
        {
            ShouldListenTo = source => 
                source.Name.StartsWith("Thor") || 
                source.Name.StartsWith("Microsoft.AspNetCore") ||
                source.Name.StartsWith("System.Net.Http"),
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity => 
            {
                // Console.WriteLine($"Global activity started: {activity.DisplayName}, ID: {activity.Id}");
            },
            ActivityStopped = activity => 
            {
                // Console.WriteLine($"Global activity stopped: {activity.DisplayName}, ID: {activity.Id}, Duration: {activity.Duration.TotalMilliseconds}ms");
            }
        });
    }
    
    /// <summary>
    /// 监听所有诊断事件的订阅者
    /// </summary>
    private class AllDiagnosticListener : IObserver<DiagnosticListener>
    {
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        
        public void OnNext(DiagnosticListener listener)
        {
            // Console.WriteLine($"Diagnostic listener found: {listener.Name}");
            listener.Subscribe(new DiagnosticObserver(listener.Name));
        }
    }
    
    /// <summary>
    /// 诊断事件观察者
    /// </summary>
    private class DiagnosticObserver : IObserver<KeyValuePair<string, object?>>
    {
        private readonly string _listenerName;
        
        public DiagnosticObserver(string listenerName)
        {
            _listenerName = listenerName;
        }
        
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        
        public void OnNext(KeyValuePair<string, object?> value)
        {
            // 我们记录与 Activity 或 HTTP 请求相关的事件
            if (value.Key.Contains("Activity") || 
                value.Key.Contains("Request") || 
                value.Key.Contains("Http"))
            {
                // Console.WriteLine($"[{_listenerName}] Event: {value.Key}");
            }
        }
    }
} 