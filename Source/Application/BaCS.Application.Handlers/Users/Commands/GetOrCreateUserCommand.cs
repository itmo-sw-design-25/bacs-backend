namespace BaCS.Application.Handlers.Users.Commands;

using Abstractions.Persistence;
using Contracts.Dto;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetOrCreateUserCommand
{
    public record Query(Guid UserId, string Email, string Name, string ImageUrl) : IRequest<UserDto>;

    internal class Handler(IBaCSDbContext dbContext, IMediator mediator, IMapper mapper)
        : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is not null) return mapper.Map<UserDto>(user);

            var newUser = await mediator.Send(
                new CreateUserCommand.Command(request.UserId, request.Email, request.Name, request.ImageUrl),
                cancellationToken
            );

            return mapper.Map<UserDto>(newUser);
        }
    }
}
