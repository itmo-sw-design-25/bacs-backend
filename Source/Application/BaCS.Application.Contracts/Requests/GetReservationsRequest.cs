namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

public record GetReservationsRequest(
    [property: Description("Фильтр по ID резерваций.")]
    Guid[] Ids,
    [property: Description("Фильтр по ID пользователей.")]
    Guid[] UserIds,
    [property: Description("Фильтр по ID локаций.")]
    Guid[] LocationIds,
    [property: Description("Фильтр по ID ресурсов.")]
    Guid[] ResourceIds,
    [property: Description("Фильтр по статусам резерваций.")]
    ReservationStatus[] Statuses,
    [property: Description("Фильтр по времени начала бронирования (включительно).")]
    DateTime? AfterDate,
    [property: Description("Фильтр по времени окончания бронирования (включительно).")]
    DateTime? BeforeDate,
    int Offset = 0,
    int Limit = 10
);

public class GetReservationsRequestValidator : AbstractValidator<GetReservationsRequest>
{
    public GetReservationsRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).InclusiveBetween(0, 100);
        RuleForEach(x => x.Statuses).IsInEnum().When(x => x.Statuses is { Length: > 0 });
    }
}
