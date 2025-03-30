namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using MediatR;

public static class DeleteLocationAdminCommand
{
    public record Command(Guid LocationId, Guid AdminId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext) : IRequestHandler<Command>
    {
        public Task Handle(Command request, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
