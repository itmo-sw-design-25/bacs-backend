namespace BaCS.Application.Abstractions.Services;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTime Today { get; }
}
