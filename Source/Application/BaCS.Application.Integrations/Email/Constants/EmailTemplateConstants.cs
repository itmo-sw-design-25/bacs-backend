namespace BaCS.Application.Integrations.Email.Constants;

public static class EmailTemplateConstants
{
    public const string ReservationCreated = $"{BasePath}.ReservationCreated.html";
    public const string ReservationUpdated = $"{BasePath}.ReservationUpdated.html";
    public const string ReservationCancelled = $"{BasePath}.ReservationCancelled.html";

    private const string BasePath = "BaCS.Application.Integrations.Email.Templates";
}
