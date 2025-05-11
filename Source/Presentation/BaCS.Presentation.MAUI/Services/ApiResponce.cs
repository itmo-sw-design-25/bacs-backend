namespace BaCS.Presentation.MAUI.Services;

public class ApiResponce<T>
{
    public bool IsSuccess { get; }

    public Exception? Exception { get; } = null;

    public T? Payload { get; }

    public ApiResponce(T? payload)
    {
        IsSuccess = true;
        Payload = payload;
    }

    public ApiResponce(Exception exception)
    {
        Exception = exception;
    }
}

