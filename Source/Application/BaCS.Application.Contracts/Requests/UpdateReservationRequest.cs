namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record UpdateReservationRequest(
    [property: Description("Время начала резервации.")]
    DateTime From,
    [property: Description("Время окончания резервации.")]
    DateTime To
);

public class UpdateReservationRequestValidator : AbstractValidator<UpdateReservationRequest>
{
    public UpdateReservationRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.From).LessThan(x => x.To);
    }
}
