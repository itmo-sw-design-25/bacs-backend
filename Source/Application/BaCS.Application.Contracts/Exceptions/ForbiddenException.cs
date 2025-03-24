namespace BaCS.Application.Contracts.Exceptions;

public class ForbiddenException : ApplicationException
{
    public ForbiddenException(string message)
        : base(message) { }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException) { }
}
