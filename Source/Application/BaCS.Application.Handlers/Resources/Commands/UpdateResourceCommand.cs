namespace BaCS.Application.Handlers.Resources.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;

public static class UpdateResourceCommand
{
    public record Command(
        Guid ResourceId,
        string Name,
        string Description,
        int Floor,
        string[] Equipment,
        ResourceType Type
    ) : IRequest<ResourceDto>;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<Command, ResourceDto>
    {
        public async Task<ResourceDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources.FindAsync([request.ResourceId], cancellationToken)
                           ?? throw new EntityNotFoundException<Resource>(request.ResourceId);

            if (currentUser.IsAdminIn(resource.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для обновления ресурса");

            resource.Name = request.Name;
            resource.Description = request.Description;
            resource.Floor = request.Floor;
            resource.Equipment = request.Equipment;
            resource.Type = request.Type;

            dbContext.Resources.Update(resource);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ResourceDto>(resource);
        }
    }
}
