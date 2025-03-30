namespace BaCS.Application.Handlers.Resources.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Responces;
using Domain.Core.Enums;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetResourcesQuery
{
    public record Query(
        Guid[] Ids,
        Guid[] LocationIds,
        ResourceType[] Types,
        int Skip,
        int Take
    ) : IRequest<PaginatedResponse<ResourceDto>>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, PaginatedResponse<ResourceDto>>
    {
        public async Task<PaginatedResponse<ResourceDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Resources.AsNoTracking();

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            if (request.LocationIds is { Length: > 0 })
            {
                query = query.Where(x => request.LocationIds.Contains(x.LocationId));
            }

            if (request.Types is { Length: > 0 })
            {
                query = query.Where(x => request.Types.Contains(x.Type));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var resources = query.OrderBy(x => x.CreatedAt).Skip(request.Skip).Take(request.Take);

            var resourceDtos = await mapper.From(resources).ProjectToType<ResourceDto>().ToListAsync(cancellationToken);

            return new PaginatedResponse<ResourceDto>
            {
                Collection = resourceDtos,
                TotalCount = totalCount
            };
        }
    }
}
