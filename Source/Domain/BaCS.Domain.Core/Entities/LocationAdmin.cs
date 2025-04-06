namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class LocationAdmin : AuditableEntity
{
    public Guid UserId { get; init; }
    public Guid LocationId { get; init; }

    public virtual User User { get; init; }
    public virtual Location Location { get; init; }
}
