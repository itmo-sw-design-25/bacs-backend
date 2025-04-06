namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Constants;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MediatR;

public static class DeleteLocationImageCommand
{
    public record Command(Guid LocationId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IFileStorage fileStorage, ICurrentUser currentUser)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для удаления фотографии локации");

            var location = await dbContext.Locations.FindAsync([request.LocationId], cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            if (location.ImageUrl is null) return;

            var result = await fileStorage.DeleteImage(location.ImageUrl, BucketNames.Locations, cancellationToken);

            if (result.Success is false)
                throw new ImageUploadException($"Не удалось удалить фотографию локации с ID {request.LocationId}");

            location.ImageUrl = null;

            dbContext.Locations.Update(location);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
