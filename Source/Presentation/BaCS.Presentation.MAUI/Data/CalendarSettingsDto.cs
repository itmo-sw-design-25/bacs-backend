namespace BaCS.Presentation.MAUI.Data;

using System.ComponentModel;
using Enums;

public record CalendarSettingsDto(
    [property: Description("Время начала работы локации.")]
    TimeOnly AvailableFrom,
    [property: Description("Время окончания работы локации.")]
    TimeOnly AvailableTo,
    [property: Description("Набор рабочих дней недели локации.")]
    RussianDayOfWeek[] AvailableDaysOfWeek
);
