namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeleteLocationAdminCommand
{
    public record Command(Guid LocationId, Guid AdminId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для удаления администратора данной локации");

            var location = await dbContext
                               .Locations
                               .Include(x => x.Admins)
                               .SingleOrDefaultAsync(x => x.Id == request.LocationId, cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            var adminToRemove = location.Admins.FirstOrDefault(u => u.Id == request.AdminId);

            if (adminToRemove is null)
            {
                throw new BusinessRulesException(
                    $"Пользователь с ID {request.AdminId} не является администратором данной локации"
                );
            }

            location.Admins.Remove(adminToRemove);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
