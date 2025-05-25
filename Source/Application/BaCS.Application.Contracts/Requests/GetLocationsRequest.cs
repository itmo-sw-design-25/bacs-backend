namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record GetLocationsRequest(
    [property: Description("Фильтр по ID локаций.")]
    Guid[] Ids,
    int Offset = 0,
    int Limit = 10
);

public class GetLocationsRequestValidator : AbstractValidator<GetLocationsRequest>
{
    public GetLocationsRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).InclusiveBetween(0, 100);
    }
}
