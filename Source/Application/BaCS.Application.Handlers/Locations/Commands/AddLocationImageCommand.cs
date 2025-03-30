namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Contracts.Constants;
using Contracts.Exceptions;
using Domain.Core.ValueObjects;
using MediatR;

public static class AddLocationImageCommand
{
    public record Command(Guid LocationId, ImageInfo ImageInfo) : IRequest<string>;

    internal class Handler(IBaCSDbContext dbContext, IFileStorage fileStorage) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var location = await dbContext.Locations
                               .FindAsync([request.LocationId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Локация с ID {request.LocationId} не найдена.");

            var result = await fileStorage.UploadImage(request.ImageInfo, BucketNames.Locations, cancellationToken);

            if (result.Success is false)
                throw new ImageUploadException($"Не удалось загрузить фотографию для локации с ID {request.LocationId}");

            location.ImageUrl = result.Path;

            dbContext.Locations.Update(location);
            await dbContext.SaveChangesAsync(cancellationToken);

            return result.Path;
        }
    }
}
