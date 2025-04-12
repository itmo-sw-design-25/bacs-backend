namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
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

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<Command, LocationDto>
    {
        public async Task<LocationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для обновления локации");

            var location = await dbContext.Locations.FindAsync([request.LocationId], cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            location.Name = request.Name;
            location.Address = request.Address;
            location.Description = request.Description;
            location.CalendarSettings.AvailableFrom = request.CalendarSettings.AvailableFrom;
            location.CalendarSettings.AvailableTo = request.CalendarSettings.AvailableTo;
            location.CalendarSettings.AvailableDaysOfWeek = request.CalendarSettings.AvailableDaysOfWeek;

            dbContext.Locations.Update(location);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<LocationDto>(location);
        }
    }
}
