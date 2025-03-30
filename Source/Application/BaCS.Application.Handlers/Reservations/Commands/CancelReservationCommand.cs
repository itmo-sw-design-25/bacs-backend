namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Contracts.Exceptions;
using Domain.Core.Enums;
using MediatR;

public static class CancelReservationCommand
{
    public record Command(Guid ReservationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext) : IRequestHandler<Command>
    {
        // TODO: семафор должен быть глобальным для всех операций над резервациями
        private static readonly SemaphoreSlim Semaphore = new(1);

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations
                                  .FindAsync([request.ReservationId], cancellationToken: cancellationToken)
                              ?? throw new NotFoundException($"Резервация с ID {request.ReservationId} не найдена.");

            await Semaphore.WaitAsync(cancellationToken);

            reservation.Status = ReservationStatus.Cancelled;

            dbContext.Reservations.Update(reservation);
            await dbContext.SaveChangesAsync(cancellationToken);

            Semaphore.Release();
        }
    }
}
