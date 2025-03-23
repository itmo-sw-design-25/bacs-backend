namespace BaCS.Domain.Core.ValueObjects;

public record TimeInterval
{
    public TimeOnly From { get; init; }
    public TimeOnly To { get; init; }
}
