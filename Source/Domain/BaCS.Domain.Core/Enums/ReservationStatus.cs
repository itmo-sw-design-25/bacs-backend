namespace BaCS.Domain.Core.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ReservationStatus>))]
public enum ReservationStatus
{
    Created = 0,
    Cancelled
}
