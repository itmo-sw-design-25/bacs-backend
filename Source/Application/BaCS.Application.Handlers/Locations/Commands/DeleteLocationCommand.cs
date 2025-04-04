namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeleteLocationCommand
{
    public record Command(Guid LocationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IDateTimeService dateTimeService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var location = await dbContext.Locations.FindAsync([request.LocationId], cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            var now = dateTimeService.UtcNow;
            var hasActiveReservations = await dbContext.Reservations.AnyAsync(
                x => x.LocationId == location.Id && x.To >= now,
                cancellationToken
            );

            if (hasActiveReservations)
            {
                throw new BusinessRulesException(
                    $"Локацию с ID {location.Id} нельзя удалить, т.к. в ней присутствуют активные бронирования."
                );
            }

            dbContext.Locations.Remove(location);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
