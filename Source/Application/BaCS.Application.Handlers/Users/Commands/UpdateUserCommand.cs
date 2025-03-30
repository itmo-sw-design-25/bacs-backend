namespace BaCS.Application.Handlers.Users.Commands;

using Abstractions.Persistence;
using Contracts.Dto;
using MediatR;

public static class UpdateUserCommand
{
    public record Command(Guid UserId, string Name, string Email) : IRequest<UserDto>;

    internal class Handler(IBaCSDbContext dbContext) : IRequestHandler<Command, UserDto>
    {
        public Task<UserDto> Handle(Command request, CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }
}
