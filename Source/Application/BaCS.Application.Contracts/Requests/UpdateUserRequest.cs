namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record UpdateUserRequest(
    [property: Description("Email-адрес пользователя.")]
    string Email,
    [property: Description("Уведомлять о бронированиях по электронной почте.")]
    bool EnableEmailNotifications
);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.EnableEmailNotifications).NotNull();
    }
}
