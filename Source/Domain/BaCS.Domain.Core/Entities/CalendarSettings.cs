namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class CalendarSettings : UpdatableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid LocationId { get; init; }

    public TimeOnly AvailableFrom { get; init; }
    public TimeOnly AvailableTo { get; init; }
    public RussianDayOfWeek[] AvailableDaysOfWeek { get; init; } = Enum.GetValues<RussianDayOfWeek>();

    public virtual Location Location { get; init; }
}
