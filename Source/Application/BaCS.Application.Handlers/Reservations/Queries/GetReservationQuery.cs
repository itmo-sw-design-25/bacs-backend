namespace BaCS.Application.Handlers.Reservations.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using MapsterMapper;
using MediatR;

public static class GetReservationQuery
{
    public record Query(Guid ReservationId) : IRequest<ReservationDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, ReservationDto>
    {
        public async Task<ReservationDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var reservation = await dbContext
                                  .Reservations
                                  .FindAsync([request.ReservationId], cancellationToken: cancellationToken)
                              ?? throw new NotFoundException($"Локация с ID {request.ReservationId} не найдена.");

            return mapper.Map<ReservationDto>(reservation);
        }
    }
}
