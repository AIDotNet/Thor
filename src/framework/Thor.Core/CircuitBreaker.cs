namespace Thor.Core;

public class CircuitBreaker(int failureThreshold, TimeSpan openTimeSpan)
{
    private readonly object _syncLock = new();
    private int _failureCount;

    private CircuitBreakerState _state = CircuitBreakerState.Closed;
    private DateTime _nextRetryTime = DateTime.MinValue;

    public async ValueTask ExecuteAsync(Func<Task> action, int maxAttempts,int delay = 500)
    {
        lock (_syncLock)
        {
            if (_state == CircuitBreakerState.Open && DateTime.UtcNow >= _nextRetryTime)
            {
                _state = CircuitBreakerState.HalfOpen;
            }

            if (_state == CircuitBreakerState.Open)
            {
                throw new CircuitBreakerOpenException("Circuit breaker is open and requests are not allowed.");
            }
        }

        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;
            try
            {
                await action();

                lock (_syncLock)
                {
                    _failureCount = 0; // Reset the failure count
                    if (_state == CircuitBreakerState.HalfOpen)
                    {
                        _state = CircuitBreakerState.Closed;
                    }
                }

                return; // Exit if action is successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");

                lock (_syncLock)
                {
                    _failureCount++;
                    if (_failureCount >= failureThreshold)
                    {
                        _state = CircuitBreakerState.Open;
                        _nextRetryTime = DateTime.UtcNow.Add(openTimeSpan);
                    }
                }

                if (attempts >= maxAttempts)
                {
                    throw; // Re-throw the last exception after max attempts reached
                }
            }

            await Task.Delay(delay); // Optionally wait before retrying, adjust as necessary
        }
    }

    private enum CircuitBreakerState
    {
        Closed,
        Open,
        HalfOpen
    }

    public class CircuitBreakerOpenException(string message) : Exception(message);
}