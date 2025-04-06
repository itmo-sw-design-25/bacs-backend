namespace BaCS.Domain.Core.ValueObjects;

public record DateTimeInterval(DateTime From, DateTime To)
{
    public bool Empty => From == To;
    public TimeSpan Duration => To - From;
};
