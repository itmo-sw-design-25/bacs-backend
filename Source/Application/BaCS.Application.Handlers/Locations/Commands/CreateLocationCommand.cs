namespace BaCS.Application.Handlers.Locations.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;

public static class CreateLocationCommand
{
    public record Command(
        string Name,
        string Address,
        string Description,
        CalendarSettingsDto CalendarSettings
    ) : IRequest<LocationDto>;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<Command, LocationDto>
    {
        public async Task<LocationDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.IsSuperAdmin() is false)
                throw new ForbiddenException("Недостаточно прав для создания локации");

            var location = mapper.Map<Location>(request);

            await dbContext.Locations.AddAsync(location, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<LocationDto>(location);
        }
    }
}
