namespace BaCS.Application.Services.Services;

using System.Security.Claims;
using Abstractions.Services;
using Contracts.Constants;
using Microsoft.AspNetCore.Http;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal _principal = httpContextAccessor.HttpContext?.User!;

    public Guid UserId
    {
        get
        {
            var id = _principal.FindFirst(ApplicationClaimDefaults.IdClaimType)!;

            return Guid.Parse(id.Value);
        }
    }

    public string Email => _principal.FindFirst(ApplicationClaimDefaults.EmailClaimType)!.Value;

    public bool IsAdminIn(params Guid[] locationIds)
    {
        if (locationIds is not { Length: > 0 }) return false;

        var roleClaims = _principal.FindAll(ApplicationClaimDefaults.RolesClaimType);

        var isLocationsAdmin = locationIds.All(
            id => roleClaims.Any(claim => claim.Value == $"{ApplicationClaimRoles.AdminRolePrefix}{id}")
        );

        return IsSuperAdmin() || isLocationsAdmin;
    }

    public bool IsSuperAdmin()
    {
        var roleClaims = _principal.FindAll(ApplicationClaimDefaults.RolesClaimType);

        return roleClaims.Any(x => x.Value == ApplicationClaimRoles.SuperAdminRole);
    }
}
