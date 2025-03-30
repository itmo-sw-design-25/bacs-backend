namespace BaCS.Application.Handlers.Locations.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetLocationQuery
{
    public record Query(Guid LocationId) : IRequest<LocationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, LocationDto>
    {
        public async Task<LocationDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var location = await dbContext.Locations
                               .FindAsync([request.LocationId], cancellationToken)
                           ?? throw new NotFoundException($"Локация с ID {request.LocationId} не найдена.");

            return mapper.Map<LocationDto>(location);
        }
    }
}
