namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;

public record CreateReservationCommand(
    [property: Description("ID ресурса для бронирования.")]
    Guid ResourceId,
    [property: Description("ID локации, в которой находится ресурс.")]
    Guid LocationId,
    [property: Description("Дата и время начала бронирования.")]
    DateTime From,
    [property: Description("Дата и время окончания бронирования.")]
    DateTime To
);
