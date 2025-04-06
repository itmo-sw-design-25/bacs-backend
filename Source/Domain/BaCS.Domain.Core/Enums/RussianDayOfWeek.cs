namespace BaCS.Domain.Core.Enums;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<RussianDayOfWeek>))]
public enum RussianDayOfWeek
{
    [Display(Name = "Понедельник")]
    Monday = 0,
    [Display(Name = "Вторник")]
    Tuesday,
    [Display(Name = "Среда")]
    Wednesday,
    [Display(Name = "Четверг")]
    Thursday,
    [Display(Name = "Пятница")]
    Friday,
    [Display(Name = "Суббота")]
    Saturday,
    [Display(Name = "Воскресенье")]
    Sunday
}
