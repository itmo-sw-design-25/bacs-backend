namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using Domain.Core.Enums;

public record UpdateResourceCommand(
    [property: Description("Название ресурса.")]
    string Name,
    [property: Description("Описание ресурса.")]
    string Description,
    [property: Description("Оборудование, прикреплённое к ресурсу.")]
    string[] Equipment,
    [property: Description("Тип ресурса.")]
    ResourceType Type
);
