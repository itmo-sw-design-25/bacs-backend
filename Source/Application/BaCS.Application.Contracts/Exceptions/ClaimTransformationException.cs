namespace BaCS.Application.Contracts.Exceptions;

public class ClaimTransformationException : UnauthorizedException
{
    public ClaimTransformationException() { }

    public ClaimTransformationException(string message)
        : base(message) { }

    public ClaimTransformationException(string message, Exception innerException)
        : base(message, innerException) { }
}
