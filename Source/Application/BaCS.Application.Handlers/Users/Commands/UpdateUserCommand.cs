namespace BaCS.Application.Handlers.Users.Commands;

using Abstractions.Persistence;
using Abstractions.Services;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;

public static class UpdateUserCommand
{
    public record Command(Guid UserId, string Email, bool EnableEmailNotifications) : IRequest<UserDto>;

    internal class Handler(IBaCSDbContext dbContext, ICurrentUser currentUser, IMapper mapper)
        : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId != request.UserId)
                throw new ForbiddenException("Недостаточно прав для обновления данных другого пользователя");

            var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken)
                       ?? throw new EntityNotFoundException<User>(request.UserId);

            user.Email = request.Email;
            user.EnableEmailNotifications = request.EnableEmailNotifications;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<UserDto>(user);
        }
    }
}
