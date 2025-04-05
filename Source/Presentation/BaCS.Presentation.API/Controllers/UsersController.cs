namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Dto;
using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Handlers.Users.Commands;
using Application.Handlers.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [EndpointSummary("Получить список пользователей.")]
    [HttpGet]
    [ProducesResponseType<PaginatedResponse<UserDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery.Query(request.Ids, request.Offset, request.Limit);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [EndpointSummary("Получить пользователя по ID.")]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> GetUser(
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserQuery.Query(userId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Обновить пользователя.")]
    [HttpPut("{userId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> UpdateUser(
        [Description("ID пользователя.")] Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateUserCommand.Command(userId, request.EnableEmailNotifications);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Удалить пользователя.")]
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> DeleteUser(
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteUserCommand.Command(userId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
