namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class Reservation : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid ResourceId { get; init; }
    public Guid LocationId { get; init; }
    public Guid UserId { get; set; }

    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public ReservationStatus Status { get; set; }

    public virtual User User { get; init; }
    public virtual Location Location { get; init; }
    public virtual Resource Resource { get; init; }
}
