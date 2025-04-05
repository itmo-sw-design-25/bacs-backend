namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using Domain.Core.Enums;
using FluentValidation;

public record UpdateResourceRequest(
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

public class UpdateResourceRequestValidator : AbstractValidator<UpdateResourceRequest>
{
    public UpdateResourceRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Type).NotNull().IsInEnum();
        RuleFor(x => x.Floor).NotNull();
        RuleForEach(x => x.Equipment).NotEmpty().When(x => x.Equipment is { Length: > 0 });
    }
}
