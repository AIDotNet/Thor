namespace Thor.Service.Model;

public sealed class ResultDto
{
    private ResultDto(string message, bool success, object? data)
    {
        Message = message;
        Success = success;
        Data = data;
    }

    public string Message { get; set; }

    public bool Success { get; set; }

    public object? Data { get; set; }

    public static ResultDto CreateSuccess(string message, object? data = null)
    {
        return new ResultDto(message, true, data);
    }

    public static ResultDto CreateFail(string message, object? data = null)
    {
        return new ResultDto(message, false, data);
    }
}

public sealed class ResultDto<T>
{
    private ResultDto(string message, bool success, T? data)
    {
        Message = message;
        Success = success;
        Data = data;
    }

    public string Message { get; set; }

    public bool Success { get; set; }

    public T? Data { get; set; }

    public static ResultDto<T> CreateSuccess(T? data = default)
    {
        return new ResultDto<T>(string.Empty, true, data);
    }

    public static ResultDto<T> CreateFail(string message, T? data = default)
    {
        return new ResultDto<T>(message, false, data);
    }
}