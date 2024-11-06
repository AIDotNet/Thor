using Thor.Abstractions.Exceptions;

namespace Thor.Core;

public class CircuitBreaker(int failureThreshold, TimeSpan openTimeSpan)
{
    private int _failureCount;

    private CircuitBreakerState _state = CircuitBreakerState.Closed;
    private DateTime _nextRetryTime = DateTime.MinValue;

    public async ValueTask ExecuteAsync(Func<Task> action, int maxAttempts, int delay = 500)
    {
        if (_state == CircuitBreakerState.Open && DateTime.UtcNow >= _nextRetryTime)
        {
            _state = CircuitBreakerState.HalfOpen;
        }

        if (_state == CircuitBreakerState.Open)
        {
            throw new CircuitBreakerOpenException("Circuit breaker is open and requests are not allowed.");
        }

        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;
            try
            {
                await action().ConfigureAwait(false);

                _failureCount = 0; // Reset the failure count
                if (_state == CircuitBreakerState.HalfOpen)
                {
                    _state = CircuitBreakerState.Closed;
                }

                return; // Exit if action is successful
            }
            catch (ThorRateLimitException)
            {
                throw new ThorRateLimitException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");

                _failureCount++;
                if (_failureCount >= failureThreshold)
                {
                    _state = CircuitBreakerState.Open;
                    _nextRetryTime = DateTime.UtcNow.Add(openTimeSpan);
                }

                if (attempts >= maxAttempts)
                {
                    throw; // 
                }
            }

            await Task.Delay(delay); // 重试延迟，避免瞬间大量请求
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