namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
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
    int Offset = 0,
    int Limit = 10
);

public class GetReservationsRequestValidator : AbstractValidator<GetReservationsRequest>
{
    public GetReservationsRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).InclusiveBetween(0, 100);
    }
}
