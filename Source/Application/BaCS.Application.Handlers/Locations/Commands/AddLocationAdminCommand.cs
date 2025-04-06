namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AddLocationAdminCommand
{
    public record Command(Guid LocationId, Guid AdminId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для добавления администратора данной локации");

            var location = await dbContext
                               .Locations
                               .Include(x => x.Admins)
                               .SingleOrDefaultAsync(x => x.Id == request.LocationId, cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            var adminExists = location.Admins.Any(x => x.Id == request.AdminId);

            if (adminExists)
            {
                throw new BusinessRulesException(
                    $"Пользователь {request.AdminId} уже является администратором данной локации"
                );
            }

            var admin = new LocationAdmin
            {
                LocationId = request.LocationId,
                UserId = request.AdminId
            };

            await dbContext.LocationAdmins.AddAsync(admin, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
