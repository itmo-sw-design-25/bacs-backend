namespace BaCS.Application.Handlers.Authorization;

using System.Security.Claims;
using Contracts.Constants;
using Contracts.Dto;
using Contracts.Exceptions;
using Contracts.Extensions;
using Users.Commands;
using Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

public class ApplicationClaimRolesHandler(IMediator mediator, IMemoryCache cache) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identities
            .FirstOrDefault(x => x.AuthenticationType == "AuthenticationTypes.Federation");

        if (identity is null) return principal;

        var user = await GetUser(identity);

        var locationIds = await mediator.Send(new GetUserAdminInQuery.Query(user.Id));

        foreach (var locationId in locationIds)
        {
            identity.AddClaim(
                new Claim(
                    ApplicationClaimDefaults.RolesClaimType,
                    $"{ApplicationClaimRoles.AdminRolePrefix}{locationId}"
                )
            );
        }

        if (identity.IsSuperAdmin())
        {
            identity.AddClaim(new Claim(ApplicationClaimDefaults.RolesClaimType, ApplicationClaimRoles.SuperAdminRole));
        }

        return principal;
    }

    private async Task<UserDto> GetUser(ClaimsIdentity identity)
    {
        var userInfo = identity.GetUserInfo();

        try
        {
            return await cache.GetOrCreateAsync(
                $"user_{userInfo.Id}",
                async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                    return await mediator.Send(
                        new GetOrCreateUserCommand.Query(userInfo.Id, userInfo.Email, userInfo.Name, userInfo.ImageUrl)
                    );
                }
            );
        }
        catch (Exception ex)
        {
            throw new ClaimTransformationException("Не удалось аутентифицировать пользователя", ex);
        }
    }
}
