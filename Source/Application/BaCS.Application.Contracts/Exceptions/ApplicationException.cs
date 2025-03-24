namespace BaCS.Application.Contracts.Exceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException() { }

    protected ApplicationException(string message)
        : base(message) { }

    protected ApplicationException(string message, Exception innerException)
        : base(message, innerException) { }
}
