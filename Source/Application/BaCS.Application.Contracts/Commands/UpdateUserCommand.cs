namespace BaCS.Application.Contracts.Commands;

using System.ComponentModel;

public record UpdateUserCommand(
    [property: Description("ФИО пользователя.")]
    string Name,
    [property: Description("Email-адрес пользователя.")]
    string Email
);
