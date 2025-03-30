namespace BaCS.Application.Handlers.Resources.Commands;

using Abstractions;
using Abstractions.Persistence;
using Contracts.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeleteResourceCommand
{
    public record Command(Guid ResourceId) : IRequest;

    internal class Handler(IBaCSDbContext dbContext, IDateTimeService dateTimeService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var resource = await dbContext.Resources
                               .FindAsync([request.ResourceId], cancellationToken: cancellationToken)
                           ?? throw new NotFoundException($"Ресурс с ID {request.ResourceId} не найден.");

            var now = dateTimeService.UtcNow;
            var hasActiveReservations = await dbContext.Reservations.AnyAsync(
                x => x.ResourceId == resource.Id && x.To >= now,
                cancellationToken: cancellationToken
            );

            if (hasActiveReservations)
            {
                throw new ValidationException(
                    $"Ресурс с ID {resource.Id} нельзя удалить, т.к. на нём присутствуют активные бронирования."
                );
            }

            dbContext.Resources.Remove(resource);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
