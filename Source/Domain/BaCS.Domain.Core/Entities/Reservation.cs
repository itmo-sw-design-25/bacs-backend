namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class Reservation : UpdatableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid ResourceId { get; init; }
    public Guid LocationId { get; init; }
    public Guid UserId { get; init; }

    public DateTime From { get; init; }
    public DateTime To { get; init; }
    public ReservationStatus Status { get; init; }

    public virtual User User { get; init; }
    public virtual Location Location { get; init; }
    public virtual Resource Resource { get; init; }
}
