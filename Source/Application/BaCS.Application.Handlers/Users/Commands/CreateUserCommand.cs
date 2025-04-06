namespace BaCS.Application.Handlers.Users.Commands;

using Abstractions.Persistence;
using Contracts.Dto;
using Contracts.Exceptions;
using Domain.Core.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CreateUserCommand
{
    public record Command(Guid UserId, string Email, string Name, string ImageUrl) : IRequest<UserDto>;

    internal class Handler(IBaCSDbContext dbContext, IMapper mapper) : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var emailExists = await dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
                throw new BusinessRulesException($"Пользователь с email {request.Email} уже существует в системе.");

            var user = mapper.Map<User>(request);
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<UserDto>(user);
        }
    }
}
