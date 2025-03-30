namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record GetLocationsRequest(
    [property: Description("Фильтр по ID резерваций.")]
    Guid[] Ids,
    int Skip = 0,
    int Take = 10
);

public class GetLocationsRequestValidator : AbstractValidator<GetLocationsRequest>
{
    public GetLocationsRequestValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThanOrEqualTo(0);
    }
}
