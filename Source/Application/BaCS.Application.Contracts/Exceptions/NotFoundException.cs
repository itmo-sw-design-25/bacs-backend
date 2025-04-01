namespace BaCS.Application.Contracts.Exceptions;

public abstract class NotFoundException : ApplicationException
{
    protected NotFoundException(string message)
        : base(message) { }

    protected NotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
