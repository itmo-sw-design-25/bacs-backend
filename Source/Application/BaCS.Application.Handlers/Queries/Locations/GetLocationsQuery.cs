namespace BaCS.Application.Handlers.Queries.Locations;

using Abstractions;
using Contracts.Dto;
using Contracts.Responces;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetLocationsQuery
{
    public record Query(Guid[] LocationIds, int Skip, int Take) : IRequest<PaginatedResponse<LocationDto>>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, PaginatedResponse<LocationDto>>
    {
        public async Task<PaginatedResponse<LocationDto>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var query = dbContext.Locations.AsNoTracking();

            if (request.LocationIds is { Length: > 0 })
            {
                query = query.Where(x => request.LocationIds.Contains(x.Id));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var locations = query.OrderBy(x => x.Name).Skip(request.Skip).Take(request.Take);

            var locationDtos = await mapper.From(locations).ProjectToType<LocationDto>().ToListAsync(cancellationToken);

            return new PaginatedResponse<LocationDto>
            {
                Collection = locationDtos,
                TotalCount = totalCount
            };
        }
    }
}
