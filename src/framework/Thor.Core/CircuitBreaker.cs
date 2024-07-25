namespace Thor.Core;

public class CircuitBreaker
{
    private readonly object syncLock = new object();
    private int failureCount = 0;
    private readonly int failureThreshold;
    private readonly TimeSpan openTimeSpan;

    private CircuitBreakerState state = CircuitBreakerState.Closed;
    private DateTime nextRetryTime = DateTime.MinValue;

    public CircuitBreaker(int failureThreshold, TimeSpan openTimeSpan)
    {
        this.failureThreshold = failureThreshold;
        this.openTimeSpan = openTimeSpan;
    }

    public void Execute(Action action, int maxAttempts,int delay = 500)
    {
        lock (syncLock)
        {
            if (state == CircuitBreakerState.Open && DateTime.UtcNow >= nextRetryTime)
            {
                state = CircuitBreakerState.HalfOpen;
            }

            if (state == CircuitBreakerState.Open)
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
                action();

                lock (syncLock)
                {
                    failureCount = 0; // Reset the failure count
                    if (state == CircuitBreakerState.HalfOpen)
                    {
                        state = CircuitBreakerState.Closed;
                    }
                }

                return; // Exit if action is successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");

                lock (syncLock)
                {
                    failureCount++;
                    if (failureCount >= failureThreshold)
                    {
                        state = CircuitBreakerState.Open;
                        nextRetryTime = DateTime.UtcNow.Add(openTimeSpan);
                    }
                }

                if (attempts >= maxAttempts)
                {
                    throw; // Re-throw the last exception after max attempts reached
                }
            }

            Thread.Sleep(delay); // Optionally wait before retrying, adjust as necessary
        }
    }

    public async ValueTask ExecuteAsync(Func<Task> action, int maxAttempts,int delay = 500)
    {
        lock (syncLock)
        {
            if (state == CircuitBreakerState.Open && DateTime.UtcNow >= nextRetryTime)
            {
                state = CircuitBreakerState.HalfOpen;
            }

            if (state == CircuitBreakerState.Open)
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

                lock (syncLock)
                {
                    failureCount = 0; // Reset the failure count
                    if (state == CircuitBreakerState.HalfOpen)
                    {
                        state = CircuitBreakerState.Closed;
                    }
                }

                return; // Exit if action is successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempts} failed: {ex.Message}");

                lock (syncLock)
                {
                    failureCount++;
                    if (failureCount >= failureThreshold)
                    {
                        state = CircuitBreakerState.Open;
                        nextRetryTime = DateTime.UtcNow.Add(openTimeSpan);
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

    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(string message) : base(message)
        {
        }
    }
}