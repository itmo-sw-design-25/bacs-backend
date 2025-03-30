namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateReservationCommand
{
    public record Command(Guid ReservationId, DateTime From, DateTime To) : IRequest<ReservationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, ReservationDto>
    {
        // TODO: семафор должен быть глобальным для всех операций над резервациями
        private static readonly SemaphoreSlim Semaphore = new(1);

        public async Task<ReservationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations
                                  .FindAsync([request.ReservationId], cancellationToken: cancellationToken)
                              ?? throw new NotFoundException($"Резервация с ID {request.ReservationId} не найдена.");

            await Semaphore.WaitAsync(cancellationToken);

            var isConflicting = await dbContext
                .Reservations
                .Where(x => x.ResourceId == reservation.ResourceId && x.Status != ReservationStatus.Cancelled)
                .AnyAsync(
                    x =>
                        (reservation.From < x.To && reservation.To > x.From)
                        || (reservation.From == x.From && reservation.To == x.To),
                    cancellationToken: cancellationToken
                );

            if (isConflicting)
            {
                throw new ValidationException(
                    $"Ресурс уже забронирован на выбранное время {reservation.From.Date:yyyy-M-d} {reservation.From:hh:mm:ss z}-{reservation.To:hh:mm:ss z}"
                );
            }

            reservation.From = request.From;
            reservation.To = request.To;

            dbContext.Reservations.Update(reservation);
            await dbContext.SaveChangesAsync(cancellationToken);

            Semaphore.Release();

            return mapper.Map<ReservationDto>(reservation);
        }
    }
}
