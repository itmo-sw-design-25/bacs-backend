namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using Dto;
using FluentValidation;

public record CreateLocationRequest(
    [property: Description("Название локации.")]
    string Name,
    [property: Description("Адресс локации.")]
    string Address,
    [property: Description("Описание локации.")]
    string Description,
    [property: Description("Настройки времени работы локации.")]
    CalendarSettingsDto CalendarSettings
);

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.CalendarSettings).NotNull();
        RuleFor(x => x.CalendarSettings.AvailableFrom).NotNull();
        RuleFor(x => x.CalendarSettings.AvailableTo).NotNull();
        RuleFor(x => x.CalendarSettings.AvailableDaysOfWeek).NotEmpty();
        RuleFor(x => x.CalendarSettings.AvailableFrom).LessThan(x => x.CalendarSettings.AvailableTo);
    }
}
