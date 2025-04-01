namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MediatR;

public static class CancelReservationCommand
{
    public record Command(Guid ReservationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations.FindAsync([request.ReservationId], cancellationToken)
                              ?? throw new EntityNotFoundException<Reservation>(request.ReservationId);

            var semaphore = GlobalSemaphores.GetForResource(reservation.ResourceId);

            try
            {
                await semaphore.WaitAsync(cancellationToken);

                reservation.Status = ReservationStatus.Cancelled;

                dbContext.Reservations.Update(reservation);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
