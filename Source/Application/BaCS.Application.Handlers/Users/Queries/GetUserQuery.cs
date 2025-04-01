namespace BaCS.Application.Handlers.Users.Queries;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;

public static class GetUserQuery
{
    public record Query(Guid UserId) : IRequest<UserDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken)
                       ?? throw new EntityNotFoundException<User>(request.UserId);

            return mapper.Map<UserDto>(user);
        }
    }
}
