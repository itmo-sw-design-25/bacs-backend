namespace BaCS.Presentation.API.Controllers;

using Application.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    [EndpointSummary("Получить список пользователей.")]
    [HttpGet]
    [ProducesResponseType<UserDto[]>(StatusCodes.Status200OK, "application/json")]
    public IActionResult GetUsers() => Ok();

    [EndpointSummary("Получить пользователя по ID.")]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetUser([FromRoute] Guid userId) => Ok();

    [EndpointSummary("Обновить пользователя.")]
    [HttpPut("{userId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateUser([FromRoute] Guid userId) => Ok();

    [EndpointSummary("Удалить пользователя.")]
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteUser([FromRoute] Guid userId) => NoContent();
}
