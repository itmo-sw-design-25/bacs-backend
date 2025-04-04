namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Constants;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;
using MediatR;

public static class AddLocationImageCommand
{
    public record Command(Guid LocationId, ImageInfo ImageInfo) : IRequest<string>;

    internal class Handler(IBaCSDbContext dbContext, IFileStorage fileStorage, ICurrentUser currentUser)
        : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для добавления фотографии локации");

            var location = await dbContext.Locations.FindAsync([request.LocationId], cancellationToken)
                           ?? throw new EntityNotFoundException<Location>(request.LocationId);

            var result = await fileStorage.UploadImage(request.ImageInfo, BucketNames.Locations, cancellationToken);

            if (result.Success is false)
            {
                throw new ImageUploadException(
                    $"Не удалось загрузить фотографию для локации с ID {request.LocationId}"
                );
            }

            location.ImageUrl = result.Path;

            dbContext.Locations.Update(location);
            await dbContext.SaveChangesAsync(cancellationToken);

            return location.ImageUrl;
        }
    }
}
