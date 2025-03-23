namespace BaCS.Application.Contracts.Dto;

using System.ComponentModel;

public record LocationDto(
    [property: Description("ID локации.")]
    Guid Id,
    [property: Description("Название локации.")]
    string Name,
    [property: Description("Адрес локации.")]
    string Address,
    [property: Description("Описание локации.")]
    string Description,
    [property: Description("Ссылка на фотографию локации.")]
    string ImageUrl,
    [property: Description("Настройки времени работы локации.")]
    CalendarSettingsDto CalendarSettings
);
