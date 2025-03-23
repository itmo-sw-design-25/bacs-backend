namespace BaCS.Application.Contracts.Queries;

using System.ComponentModel;
using Domain.Core.Enums;

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
