namespace BaCS.Application.Contracts.Extensions;

using System.Security.Claims;
using System.Text.Json;
using Constants;
using Exceptions;

public static class ClaimsIdentityExtensions
{
    public static (string Email, string Name, Guid Id, string ImageUrl) GetUserInfo(this ClaimsIdentity identity)
    {
        string email = identity.GetEmail(),
               name = identity.GetName(),
               id = identity.GetId(),
               pictureUrl = identity.GetPictureUrl();

        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(id))
            return (email, name, Guid.Parse(id), pictureUrl);

        var missingClaims = new[] { ApplicationClaimDefaults.EmailClaimType, ApplicationClaimDefaults.IdClaimType };

        throw new ClaimTransformationException(
            $"Отсутствуют claim-ы, необходимые для аутентификации: {string.Join(", ", missingClaims)}"
        );
    }

    public static bool IsSuperAdmin(this ClaimsIdentity identity)
    {
        var realmAccessRole = identity.FindFirst(ApplicationClaimDefaults.RealmRoleClaimType)?.Value;

        if (string.IsNullOrWhiteSpace(realmAccessRole)) return false;

        using var realmAccess = JsonDocument.Parse(realmAccessRole);

        var containsRoles = realmAccess.RootElement.TryGetProperty(
            "roles",
            out var rolesElement
        );

        return containsRoles && rolesElement
            .EnumerateArray()
            .Select(role => role.GetString())
            .Any(value => value == ApplicationClaimRoles.SuperAdminRoleValue);
    }

    private static string GetEmail(this ClaimsIdentity identity) =>
        identity.FindFirst(ApplicationClaimDefaults.EmailClaimType)?.Value;

    private static string GetName(this ClaimsIdentity identity) =>
        identity.FindFirst(ApplicationClaimDefaults.NameClaimType)?.Value;

    private static string GetId(this ClaimsIdentity identity) =>
        identity.FindFirst(ApplicationClaimDefaults.IdClaimType)?.Value;

    private static string GetPictureUrl(this ClaimsIdentity identity) =>
        identity.FindFirst(ApplicationClaimDefaults.PictureClaimType)?.Value;
}
