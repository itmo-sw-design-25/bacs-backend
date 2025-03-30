namespace BaCS.Application.Handlers.Commands.Users;

using Abstractions;
using MapsterMapper;
using MediatR;

public static class DeleteUserCommand
{
    public record Command(Guid UserId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Command>
    {
        public Task Handle(Command request, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
