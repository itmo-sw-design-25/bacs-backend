namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Commands;
using Application.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    [EndpointSummary("Получить список пользователей.")]
    [HttpGet]
    [ProducesResponseType<UserDto[]>(StatusCodes.Status200OK, "application/json")]
    public IActionResult GetUsers(CancellationToken cancellationToken) => Ok();

    [EndpointSummary("Получить пользователя по ID.")]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetUser(
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Обновить пользователя.")]
    [HttpPut("{userId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateUser(
        [Description("ID пользователя.")] Guid userId,
        [FromBody] UpdateUserCommand command,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Удалить пользователя.")]
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteUser(
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    ) => NoContent();
}
