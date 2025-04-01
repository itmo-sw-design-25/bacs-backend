namespace BaCS.Domain.Core.Extensions;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Enums;

public static class RussianDayOfWeekExtensions
{
    public static RussianDayOfWeek ToRussianDayOfWeek(this DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => RussianDayOfWeek.Monday,
        DayOfWeek.Tuesday => RussianDayOfWeek.Tuesday,
        DayOfWeek.Wednesday => RussianDayOfWeek.Wednesday,
        DayOfWeek.Thursday => RussianDayOfWeek.Thursday,
        DayOfWeek.Friday => RussianDayOfWeek.Friday,
        DayOfWeek.Saturday => RussianDayOfWeek.Saturday,
        DayOfWeek.Sunday => RussianDayOfWeek.Sunday,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, "Неизвестный день недели")
    };

    public static RussianDayOfWeek ToRussianDayOfWeek(this DateTime dateTime) =>
        dateTime.DayOfWeek.ToRussianDayOfWeek();

    public static string ToDisplayName(this RussianDayOfWeek dayOfWeek)
    {
        var field = dayOfWeek.GetType().GetField(dayOfWeek.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Description ?? dayOfWeek.ToString();
    }
}
