namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using Dto;

public record CreateLocationCommand(
    [property: Description("Название локации.")]
    string Name,
    [property: Description("Адресс локации.")]
    string Address,
    [property: Description("Описание локации.")]
    string Description,
    [property: Description("Настройки времени работы локации.")]
    CalendarSettingsDto CalendarSettings
);
