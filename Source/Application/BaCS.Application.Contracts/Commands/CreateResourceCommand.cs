namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using Domain.Core.Enums;

public record CreateResourceCommand(
    [property: Description("ID локации, в которой будет создан ресурс.")]
    Guid LocationId,
    [property: Description("Название ресурса.")]
    string Name,
    [property: Description("Описание ресурса.")]
    string Description,
    [property: Description("Оборудование, прикреплённое к ресурсу.")]
    string[] Equipment,
    [property: Description("Тип ресурса.")]
    ResourceType Type
);
