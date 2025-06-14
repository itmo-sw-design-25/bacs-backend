namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record CreateReservationRequest(
    [property: Description("ID ресурса для бронирования.")]
    Guid ResourceId,
    [property: Description("ID локации, в которой находится ресурс.")]
    Guid LocationId,
    [property: Description("Дата и время начала бронирования.")]
    DateTime From,
    [property: Description("Дата и время окончания бронирования.")]
    DateTime To
);

public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(x => x.ResourceId).NotEmpty();
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.From).LessThan(x => x.To);
    }
}
