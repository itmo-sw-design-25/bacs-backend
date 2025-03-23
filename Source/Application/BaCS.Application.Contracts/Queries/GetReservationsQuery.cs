namespace BaCS.Application.Contracts.Queries;

using System.ComponentModel;

public record GetReservationsQuery(
    [property: Description("Фильтр по ID резерваций.")]
    Guid[] Ids,
    [property: Description("Фильтр по ID пользователей.")]
    Guid[] UserIds,
    [property: Description("Фильтр по ID ресурсов.")]
    Guid[] ResourceIds,
    [property: Description("Фильтр по ID локаций.")]
    Guid[] LocationIds,
    int Skip = 0,
    int Take = 10
);
