namespace BaCS.Presentation.MAUI.Data.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ReservationStatus>))]
public enum ReservationStatus
{
    Created = 0,
    Cancelled
}
