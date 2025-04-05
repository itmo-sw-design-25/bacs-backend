namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

public record CreateResourceRequest(
    [property: Description("ID локации, в которой будет создан ресурс.")]
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
    ResourceType Type
);

public class CreateResourceRequestValidator : AbstractValidator<CreateResourceRequest>
{
    public CreateResourceRequestValidator()
    {
        RuleFor(x => x.LocationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Type).NotNull().IsInEnum();
        RuleFor(x => x.Floor).NotNull();
        RuleForEach(x => x.Equipment).NotEmpty().When(x => x.Equipment is { Length: > 0 });
    }
}
