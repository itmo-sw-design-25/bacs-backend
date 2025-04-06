namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using Domain.Core.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateReservationCommand
{
    public record Command(Guid ReservationId, DateTime From, DateTime To) : IRequest<ReservationDto>;

    internal class Handler(
        IBaCSDbContext dbContext,
        IReservationCalendarValidator calendarValidator,
        ICurrentUser currentUser,
        IMapper mapper
    ) : IRequestHandler<Command, ReservationDto>
    {
        public async Task<ReservationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations.FindAsync([request.ReservationId], cancellationToken)
                              ?? throw new EntityNotFoundException<Reservation>(request.ReservationId);

            var location = await dbContext.Locations.FindAsync([reservation.LocationId], cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(reservation.LocationId);

            if (reservation.UserId != currentUser.UserId && currentUser.IsAdminIn(reservation.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для обновления брони другого пользователя");

            var interval = new DateTimeInterval(request.From, request.To);
            calendarValidator.ValidateAndThrow(interval, location.CalendarSettings);

            var semaphore = GlobalSemaphores.GetForResource(reservation.ResourceId);

            try
            {
                await semaphore.WaitAsync(cancellationToken);

                var isConflicting = await dbContext
                    .Reservations
                    .Where(
                        x => x.Id != reservation.Id &&
                             x.ResourceId == reservation.ResourceId &&
                             x.Status != ReservationStatus.Cancelled
                    )
                    .AnyAsync(
                        x => (request.From < x.To && request.To > x.From) ||
                             (request.From == x.From && request.To == x.To),
                        cancellationToken
                    );

                if (isConflicting)
                {
                    throw new ReservationConflictException(
                        $"Ресурс уже забронирован на выбранное время {reservation.From.Date:yyyy-M-d} {reservation.From:hh:mm:ss z}-{reservation.To:hh:mm:ss z}"
                    );
                }

                reservation.From = request.From;
                reservation.To = request.To;

                dbContext.Reservations.Update(reservation);
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
