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
            var id = _principal.FindFirstValue(ApplicationClaimDefaults.IdClaimType)!;

            return Guid.Parse(id);
        }
    }

    public string Email => _principal.FindFirstValue(ApplicationClaimDefaults.EmailClaimType);

    public bool IsAdminIn(params Guid[] locationIds)
    {
        var roleClaims = _principal.FindAll(ApplicationClaimDefaults.RolesClaimType).ToArray();

        var isSuperAdmin = roleClaims.Any(x => x.Value == ApplicationClaimRoles.SuperAdminRole);
        var isLocationsAdmin = locationIds.All(
            id => roleClaims.Any(claim => claim.Value == $"{ApplicationClaimRoles.AdminRolePrefix}{id}")
        );

        return isSuperAdmin && isLocationsAdmin;
    }
}
