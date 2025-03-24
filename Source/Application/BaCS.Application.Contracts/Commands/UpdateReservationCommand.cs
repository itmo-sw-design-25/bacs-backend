namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using FluentValidation;

public record UpdateReservationCommand(
    [property: Description("Время начала резервации.")]
    DateTime From,
    [property: Description("Время окончания резервации.")]
    DateTime To
);

public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.From).LessThan(x => x.To);
    }
}
