namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

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

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
    public CreateResourceCommandValidator()
    {
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Type).NotNull().IsInEnum();
    }
}
