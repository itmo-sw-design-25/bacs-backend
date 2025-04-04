namespace BaCS.Application.Contracts.Dto;

using System.ComponentModel;

public record UserDto(
    [property: Description("ID пользователя.")]
    Guid Id,
    [property: Description("Email-адрес пользователя.")]
    string Email,
    [property: Description("ФИО пользователя.")]
    string Name,
    [property: Description("Ссылка на фотографию пользователя.")]
    string ImageUrl,
    [property: Description("Уведомлять о бронированиях по электронной почте.")]
    bool EnableEmailNotifications
);
