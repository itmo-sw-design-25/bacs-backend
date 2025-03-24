namespace BaCS.Application.Contracts.Queries;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

public record GetResourcesQuery(
    [property: Description("Фильтр по ID ресурсов.")]
    Guid[] Ids,
    [property: Description("Фильтр по ID локаций.")]
    Guid[] LocationIds,
    [property: Description("Фильтр по типам ресурсов.")]
    ResourceType[] Types,
    int Skip = 0,
    int Take = 10
);

public class GetResourcesQueryValidator : AbstractValidator<GetResourcesQuery>
{
    public GetResourcesQueryValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThanOrEqualTo(0);
    }
}
