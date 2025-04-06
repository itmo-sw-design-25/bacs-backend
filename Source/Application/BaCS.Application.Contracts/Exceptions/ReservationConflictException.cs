namespace BaCS.Application.Contracts.Exceptions;

public class ReservationConflictException : BusinessRulesException
{
    public ReservationConflictException() { }

    public ReservationConflictException(string message)
        : base(message) { }

    public ReservationConflictException(string message, Exception innerException)
        : base(message, innerException) { }
}
