namespace BaCS.Application.Contracts.Constants;

using System.Security.Claims;

public static class ApplicationClaimDefaults
{
    public const string IdClaimType = "sub";
    public const string EmailClaimType = "email";
    public const string NameClaimType = "name";
    public const string PictureClaimType = "picture";
    public const string RealmRoleClaimType = "realm_access";
    public const string RolesClaimType = ClaimTypes.Role;
}
