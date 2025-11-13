namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record GetReservationsReportRequest(
    [property: Description("Дата начала отчёта (включительно).")]
    DateOnly From,
    [property: Description("Дата окончания отчёта (включительно).")]
    DateOnly To,
    [property: Description("ID пользователя, по которому фильтровать бронирования.")]
    Guid? UserId,
    [property: Description("ID ресурса, по которому фильтровать бронирования.")]
    Guid? ResourceId,
    [property: Description("ID локации, по которой фильтровать бронирования.")]
    Guid? LocationId
);

public class GetReservationsReportRequestValidator : AbstractValidator<GetReservationsReportRequest>
{
    public GetReservationsReportRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.From).LessThan(x => x.To);
        RuleFor(x => x)
            .Must(x =>
                x.To.ToDateTime(TimeOnly.MinValue) - x.From.ToDateTime(TimeOnly.MinValue) < TimeSpan.FromDays(365)
            )
            .WithMessage("Период отчёта не должен превышать 365 дней.");
    }
}
