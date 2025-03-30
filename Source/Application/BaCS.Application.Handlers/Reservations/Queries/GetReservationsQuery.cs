namespace BaCS.Application.Handlers.Reservations.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Responces;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetReservationsQuery
{
    public record Query(
        Guid[] Ids,
        Guid[] UserIds,
        Guid[] ResourceIds,
        Guid[] LocationIds,
        int Skip,
        int Take
    ) : IRequest<PaginatedResponse<ReservationDto>>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, PaginatedResponse<ReservationDto>>
    {
        public async Task<PaginatedResponse<ReservationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Reservations.AsNoTracking();

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            if (request.UserIds is { Length: > 0 })
            {
                query = query.Where(x => request.UserIds.Contains(x.UserId));
            }

            if (request.LocationIds is { Length: > 0 })
            {
                query = query.Where(x => request.LocationIds.Contains(x.LocationId));
            }

            if (request.ResourceIds is { Length: > 0 })
            {
                query = query.Where(x => request.ResourceIds.Contains(x.ResourceId));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var reservations = query.OrderBy(x => x.CreatedAt).Skip(request.Skip).Take(request.Take);

            var reservationDtos =
                await mapper.From(reservations).ProjectToType<ReservationDto>().ToListAsync(cancellationToken);

            return new PaginatedResponse<ReservationDto>
            {
                Collection = reservationDtos,
                TotalCount = totalCount
            };
        }
    }
}
