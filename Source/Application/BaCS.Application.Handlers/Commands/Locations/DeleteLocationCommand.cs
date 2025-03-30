namespace BaCS.Application.Handlers.Commands.Locations;

using Abstractions;
using Contracts.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeleteLocationCommand
{
    public record Command(Guid LocationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IDateTimeService dateTimeService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var location = await dbContext.Locations
                               .FindAsync([request.LocationId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Локация с ID {request.LocationId} не найдена.");

            var now = dateTimeService.UtcNow;
            var hasActiveReservations = await dbContext.Reservations.AnyAsync(
                x => x.LocationId == location.Id && x.To >= now,
                cancellationToken: cancellationToken
            );

            if (hasActiveReservations)
            {
                throw new ValidationException(
                    $"Локацию с ID {location.Id} нельзя удалить, т.к. в ней присутствуют активные бронирования."
                );
            }

            dbContext.Locations.Remove(location);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
