namespace BaCS.Application.Handlers.Users.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Responces;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUsersQuery
{
    public record Query(Guid[] Ids, int Offset, int Limit) : IRequest<PaginatedResponse<UserDto>>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, PaginatedResponse<UserDto>>
    {
        public async Task<PaginatedResponse<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = dbContext.Users.AsNoTracking();

            if (request.Ids is { Length: > 0 })
            {
                query = query.Where(x => request.Ids.Contains(x.Id));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var users = query.OrderBy(x => x.Name).Skip(request.Offset).Take(request.Limit);

            var userDtos = await mapper.From(users).ProjectToType<UserDto>().ToListAsync(cancellationToken);

            return new PaginatedResponse<UserDto>
            {
                Collection = userDtos,
                TotalCount = totalCount
            };
        }
    }
}
