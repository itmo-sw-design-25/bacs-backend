namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;
using FluentValidation;

public record UpdateUserCommand(
    [property: Description("ФИО пользователя.")]
    string Name,
    [property: Description("Email-адрес пользователя.")]
    string Email
);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
    }
}
