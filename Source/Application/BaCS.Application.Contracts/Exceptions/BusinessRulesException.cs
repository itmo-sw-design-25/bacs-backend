namespace BaCS.Application.Contracts.Exceptions;

public class BusinessRulesException : ApplicationException
{
    public BusinessRulesException() { }

    public BusinessRulesException(string message)
        : base(message) { }

    public BusinessRulesException(string message, Exception innerException)
        : base(message, innerException) { }
}
