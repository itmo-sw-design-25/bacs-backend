namespace BaCS.Presentation.MAUI.Data.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter<RussianDayOfWeek>))]
public enum RussianDayOfWeek
{
    Monday = 0,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
