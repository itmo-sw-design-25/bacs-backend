namespace BaCS.Application.Handlers.Users.Queries;

using Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUserAdminInQuery
{
    public record Query(Guid UserId) : IRequest<IReadOnlyList<Guid>>;

    internal class Handler(IBaCSDbContext dbContext) : IRequestHandler<Query, IReadOnlyList<Guid>>
    {
        public async Task<IReadOnlyList<Guid>> Handle(Query request, CancellationToken cancellationToken) =>
            await dbContext
                .LocationAdmins
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.LocationId)
                .ToListAsync(cancellationToken);
    }
}
