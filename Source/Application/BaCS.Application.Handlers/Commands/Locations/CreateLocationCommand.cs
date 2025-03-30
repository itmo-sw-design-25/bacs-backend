namespace BaCS.Application.Handlers.Commands.Locations;

using Abstractions;
using Contracts.Dto;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;

public static class CreateLocationCommand
{
    public record Command(
        string Name,
        string Address,
        string Description,
        CalendarSettingsDto CalendarSettings
    ) : IRequest<LocationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Command, LocationDto>
    {
        public async Task<LocationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var location = mapper.Map<Location>(request);

            await dbContext.Locations.AddAsync(location, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<LocationDto>(location);
        }
    }
}
