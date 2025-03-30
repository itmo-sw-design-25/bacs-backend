namespace BaCS.Application.Handlers.Commands.Resources;

using Abstractions;
using Contracts.Dto;
using Domain.Core.Entities;
using Domain.Core.Enums;
using MapsterMapper;
using MediatR;

public static class CreateResourceCommand
{
    public record Command(
        Guid LocationId,
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
            var resource = mapper.Map<Resource>(request);

            await dbContext.Resources.AddAsync(resource, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ResourceDto>(resource);
        }
    }
}
