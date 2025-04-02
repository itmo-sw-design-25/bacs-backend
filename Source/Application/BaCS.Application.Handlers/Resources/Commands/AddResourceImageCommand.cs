namespace BaCS.Application.Handlers.Resources.Commands;

using Abstractions.Persistence;
using Contracts.Constants;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;
using MediatR;

public static class AddResourceImageCommand
{
    public record Command(Guid ResourceId, ImageInfo ImageInfo) : IRequest<string>;

    internal class Handler(IBaCSDbContext dbContext, IFileStorage fileStorage) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources.FindAsync([request.ResourceId], cancellationToken)
                           ?? throw new EntityNotFoundException<Resource>(request.ResourceId);

            var result = await fileStorage.UploadImage(request.ImageInfo, BucketNames.Resources, cancellationToken);

            if (result.Success is false)
            {
                throw new ImageUploadException(
                    $"Не удалось загрузить фотографию для ресурса с ID {request.ResourceId}"
                );
            }

            resource.ImageUrl = result.Path;

            dbContext.Resources.Update(resource);
            await dbContext.SaveChangesAsync(cancellationToken);

            return resource.ImageUrl;
        }
    }
}
