namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;

public record UpdateReservationCommand(
    [property: Description("Время начала резервации.")]
    DateTime From,
    [property: Description("Время окончания резервации.")]
    DateTime To
);
