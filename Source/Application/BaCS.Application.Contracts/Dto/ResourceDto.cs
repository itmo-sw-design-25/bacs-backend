namespace BaCS.Application.Contracts.Dto;

using System.ComponentModel;
using Domain.Core.Enums;

public record ResourceDto(
    [property: Description("ID ресурса.")]
    Guid Id,
    [property: Description("ID локации.")]
    Guid LocationId,
    [property: Description("Название ресурса.")]
    string Name,
    [property: Description("Описание ресурса.")]
    string Description,
    [property: Description("Этаж, на котором расположен ресурс.")]
    int Floor,
    [property: Description("Оборудование, прикреплённое к ресурсу.")]
    string[] Equipment,
    [property: Description("Тип ресурса.")]
    ResourceType Type,
    [property: Description("Ссылка на фотографию ресурса.")]
    string ImageUrl
);
