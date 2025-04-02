namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

public record GetResourcesRequest(
    [property: Description("Фильтр по ID ресурсов.")]
    Guid[] Ids,
    [property: Description("Фильтр по ID локаций.")]
    Guid[] LocationIds,
    [property: Description("Фильтр по типам ресурсов.")]
    ResourceType[] Types,
    int Offset = 0,
    int Limit = 10
);

public class GetResourcesRequestValidator : AbstractValidator<GetResourcesRequest>
{
    public GetResourcesRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).InclusiveBetween(0, 100);
    }
}
