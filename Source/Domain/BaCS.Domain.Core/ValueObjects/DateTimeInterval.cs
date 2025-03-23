namespace BaCS.Domain.Core.ValueObjects;

public record DateTimeInterval
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}
