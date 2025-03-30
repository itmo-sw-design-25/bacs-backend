namespace BaCS.Application.Contracts.Requests;

using System.ComponentModel;
using FluentValidation;

public record UpdateUserRequest(
    [property: Description("ФИО пользователя.")]
    string Name,
    [property: Description("Email-адрес пользователя.")]
    string Email
);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
