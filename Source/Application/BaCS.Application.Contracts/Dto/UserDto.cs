namespace BaCS.Application.Contracts.Dto;

using System.ComponentModel;

public record UserDto(
    [property: Description("ID пользователя.")]
    Guid Id,
    [property: Description("Уникальный логин пользователя.")]
    string Username,
    [property: Description("Email-адрес пользователя.")]
    string Email,
    [property: Description("ФИО пользователя.")]
    string Name
);
