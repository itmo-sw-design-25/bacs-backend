namespace BaCS.Application.Handlers.Commands.Resources;

using Abstractions;
using Contracts.Constants;
using Contracts.Exceptions;
using MediatR;

public static class DeleteResourceImageCommand
{
    public record Command(Guid ResourceId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IFileStorage fileStorage) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources
                               .FindAsync([request.ResourceId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Ресурс с ID {request.ResourceId} не найден.");

            if (resource.ImageUrl is null) return;

            var result = await fileStorage.DeleteImage(resource.ImageUrl, BucketNames.Resources, cancellationToken);

            if (result.Success is false)
                throw new ImageUploadException($"Не удалось удалить фотографию ресурса с ID {request.ResourceId}");

            resource.ImageUrl = null;

            dbContext.Resources.Update(resource);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
