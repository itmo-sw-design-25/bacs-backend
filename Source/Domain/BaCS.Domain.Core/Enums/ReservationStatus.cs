namespace BaCS.Domain.Core.Enums;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ReservationStatus>))]
public enum ReservationStatus
{
    [Display(Name = "Резервация создана")]
    Created = 0,

    [Display(Name = "Ожидает подтверждения")]
    PendingApproval = 1,

    [Display(Name = "Резервация подтверждена")]
    Accepted = 2,

    [Display(Name = "Резервация отменена")]
    Cancelled = 3
}
