namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CreateReservationCommand
{
    public record Command(Guid ResourceId, Guid LocationId, DateTime From, DateTime To)
        : IRequest<ReservationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Command, ReservationDto>
    {
        public async Task<ReservationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = mapper.Map<Reservation>(request);

            var semaphore = GlobalSemaphores.GetForResource(reservation.ResourceId);

            try
            {
                await semaphore.WaitAsync(cancellationToken);

                var isConflicting = await dbContext
                    .Reservations
                    .Where(x => x.ResourceId == reservation.ResourceId && x.Status != ReservationStatus.Cancelled)
                    .AnyAsync(
                        x =>
                            (reservation.From < x.To && reservation.To > x.From)
                            || (reservation.From == x.From && reservation.To == x.To),
                        cancellationToken
                    );

                if (isConflicting)
                {
                    throw new ReservationConflictException(
                        $"Ресурс уже забронирован на выбранное время {reservation.From.Date:yyyy-M-d} {reservation.From:hh:mm:ss z}-{reservation.To:hh:mm:ss z}"
                    );
                }

                reservation.Status = ReservationStatus.Created;
                await dbContext.Reservations.AddAsync(reservation, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }

            return mapper.Map<ReservationDto>(reservation);
        }
    }
}
