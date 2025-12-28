namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record RejectReservationRequest(
    [property: Description("Причина отклонения резервации.")]
    string Reason
);

public class RejectReservationRequestValidator : AbstractValidator<RejectReservationRequest>
{
    public RejectReservationRequestValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
