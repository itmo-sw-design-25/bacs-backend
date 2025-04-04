namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MediatR;

public static class CancelReservationCommand
{
    public record Command(Guid ReservationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations.FindAsync([request.ReservationId], cancellationToken)
                              ?? throw new EntityNotFoundException<Reservation>(request.ReservationId);

            if (reservation.UserId != currentUser.UserId && currentUser.IsAdminIn(reservation.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для отмены брони другого пользователя");

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
