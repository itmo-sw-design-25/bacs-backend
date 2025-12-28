namespace BaCS.Application.Integrations.Email.Constants;

public static class EmailTemplateConstants
{
    public const string ReservationCreated = $"{BasePath}.ReservationCreated.html";
    public const string ReservationUpdated = $"{BasePath}.ReservationUpdated.html";
    public const string ReservationCancelled = $"{BasePath}.ReservationCancelled.html";
    public const string ReservationPendingApproval = $"{BasePath}.ReservationPendingApproval.html";
    public const string ReservationApproved = $"{BasePath}.ReservationApproved.html";
    public const string ReservationRejected = $"{BasePath}.ReservationRejected.html";

    private const string BasePath = "BaCS.Application.Integrations.Email.Templates";
}
