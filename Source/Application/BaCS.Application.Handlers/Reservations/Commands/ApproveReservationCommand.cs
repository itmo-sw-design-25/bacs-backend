namespace BaCS.Application.Handlers.Reservations.Commands;

using Abstractions.Integrations;
using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ApproveReservationCommand
{
    public record Command(Guid ReservationId) : IRequest<ReservationDto>;

    internal class Handler(
        IBaCSDbContext dbContext,
        IEmailNotifier emailNotifier,
        ICurrentUser currentUser,
        IMapper mapper
    ) : IRequestHandler<Command, ReservationDto>
    {
        public async Task<ReservationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext
                                  .Reservations
                                  .Include(r => r.Location)
                                  .ThenInclude(l => l.Admins)
                                  .Include(r => r.Resource)
                                  .Include(r => r.User)
                                  .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken)
                              ?? throw new EntityNotFoundException<Reservation>(request.ReservationId);

            if (reservation.Location.Admins.All(a => a.Id != currentUser.UserId))
            {
                throw new UnauthorizedAccessException(
                    $"User {currentUser.UserId} is not an admin of location {reservation.LocationId}"
                );
            }

            if (reservation.Status != ReservationStatus.PendingApproval)
            {
                throw new InvalidOperationException(
                    $"Reservation {request.ReservationId} is not in PendingApproval status (current: {reservation.Status})"
                );
            }

            reservation.Status = ReservationStatus.Accepted;
            await dbContext.SaveChangesAsync(cancellationToken);

            await emailNotifier.SendReservationApproved(
                reservation,
                reservation.Location,
                reservation.Resource,
                reservation.User,
                cancellationToken
            );

            return mapper.Map<ReservationDto>(reservation);
        }
    }
}
