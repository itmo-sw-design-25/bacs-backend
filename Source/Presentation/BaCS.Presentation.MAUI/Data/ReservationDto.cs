namespace BaCS.Presentation.MAUI.Data;

using System.ComponentModel;
using Enums;

public record ReservationDto(
    [property: Description("ID резервации.")]
    Guid Id,
    [property: Description("ID забронированного ресурса.")]
    Guid ResourceId,
    [property: Description("ID локации, в которой забронирован ресурс.")]
    Guid LocationId,
    [property: Description("ID пользователя, забронировавшего ресурс.")]
    Guid UserId,
    [property: Description("Дата и время начала резервации.")]
    DateTime From,
    [property: Description("Дата и время окончания резервации.")]
    DateTime To,
    [property: Description("Статус резервации.")]
    ReservationStatus Status
);
