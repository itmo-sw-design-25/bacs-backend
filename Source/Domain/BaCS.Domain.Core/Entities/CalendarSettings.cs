namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class CalendarSettings : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid LocationId { get; init; }

    public TimeOnly AvailableFrom { get; set; }
    public TimeOnly AvailableTo { get; set; }
    public RussianDayOfWeek[] AvailableDaysOfWeek { get; set; } = Enum.GetValues<RussianDayOfWeek>();

    public virtual Location Location { get; init; }
}
