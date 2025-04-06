namespace BaCS.Application.Handlers.Locations.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetLocationQuery
{
    public record Query(Guid LocationId) : IRequest<LocationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Query, LocationDto>
    {
        public async Task<LocationDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var location = await dbContext
                               .Locations
                               .Include(x => x.Admins)
                               .AsNoTracking()
                               .SingleOrDefaultAsync(x => x.Id == request.LocationId, cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            return mapper.Map<LocationDto>(location);
        }
    }
}
