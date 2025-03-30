namespace BaCS.Application.Handlers.Commands.Locations;

using Abstractions;
using Contracts.Dto;
using Contracts.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateLocationCommand
{
    public record Command(
        Guid LocationId,
        string Name,
        string Address,
        string Description,
        CalendarSettingsDto CalendarSettings
    ) : IRequest<LocationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Command, LocationDto>
    {
        public async Task<LocationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var location = await dbContext.Locations
                               .FindAsync([request.LocationId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Локация с ID {request.LocationId} не найдена.");

            location.Name = request.Name;
            location.Address = request.Address;
            location.Description = request.Description;
            location.CalendarSettings.AvailableFrom = request.CalendarSettings.AvailableFrom;
            location.CalendarSettings.AvailableFrom = request.CalendarSettings.AvailableTo;
            location.CalendarSettings.AvailableDaysOfWeek = request.CalendarSettings.AvailableDaysOfWeek;

            dbContext.Locations.Update(location);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<LocationDto>(location);
        }
    }
}
