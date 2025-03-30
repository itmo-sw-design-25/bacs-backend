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
    int Skip = 0,
    int Take = 10
);

public class GetResourcesRequestValidator : AbstractValidator<GetResourcesRequest>
{
    public GetResourcesRequestValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThanOrEqualTo(0);
    }
}
