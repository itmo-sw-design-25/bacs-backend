namespace BaCS.Application.Handlers.Resources.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
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
        int Floor,
        string[] Equipment,
        ResourceType Type
    ) : IRequest<ResourceDto>;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<Command, ResourceDto>
    {
        public async Task<ResourceDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsAdminIn(request.LocationId) is false)
                throw new ForbiddenException("Недостаточно прав для добавления ресурса");

            var resource = mapper.Map<Resource>(request);

            await dbContext.Resources.AddAsync(resource, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ResourceDto>(resource);
        }
    }
}
