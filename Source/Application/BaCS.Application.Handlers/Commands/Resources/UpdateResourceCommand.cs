namespace BaCS.Application.Handlers.Commands.Resources;

using Abstractions;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;

public static class UpdateResourceCommand
{
    public record Command(
        Guid ResourceId,
        string Name,
        string Description,
        string[] Equipment,
        ResourceType Type
    ) : IRequest<ResourceDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, ResourceDto>
    {
        public async Task<ResourceDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources
                               .FindAsync([request.ResourceId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Ресурс с ID {request.ResourceId} не найден.");

            resource.Name = request.Name;
            resource.Description = request.Description;
            resource.Equipment = request.Equipment;
            resource.Type = request.Type;

            dbContext.Resources.Update(resource);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ResourceDto>(resource);
        }
    }
}
