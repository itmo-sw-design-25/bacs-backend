namespace BaCS.Application.Handlers.Resources.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Constants;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MediatR;

public static class DeleteResourceImageCommand
{
    public record Command(Guid ResourceId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IFileStorage fileStorage)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources.FindAsync([request.ResourceId], cancellationToken)
                           ?? throw new EntityNotFoundException<Resource>(request.ResourceId);

            if (currentUser.IsAdminIn(resource.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для удаления фотографии ресурса");

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
