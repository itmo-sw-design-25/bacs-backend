namespace BaCS.Domain.Core.Enums;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<ResourceType>))]
public enum ResourceType
{
    [Display(Name = "Не определён")]
    Unspecified = 0,
    [Display(Name = "Переговорная комната")]
    MeetingRoom,
    [Display(Name = "Рабочее место")]
    Workplace
}
