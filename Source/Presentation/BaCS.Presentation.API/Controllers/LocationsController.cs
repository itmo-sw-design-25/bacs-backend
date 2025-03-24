namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Commands;
using Application.Contracts.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    [AllowAnonymous]
    [EndpointSummary("Получить список локаций.")]
    [HttpGet]
    [ProducesResponseType<LocationDto[]>(StatusCodes.Status200OK, "application/json")]
    public IActionResult GetLocations(CancellationToken cancellationToken) => Ok();

    [AllowAnonymous]
    [EndpointSummary("Получить локацию по ID.")]
    [HttpGet("{locationId:guid}")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetLocation(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Создать локацию.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult CreateLocation(
        [FromBody] CreateLocationCommand command,
        CancellationToken cancellationToken
    ) => CreatedAtAction(nameof(CreateLocation), null);

    [EndpointSummary("Обновить локацию.")]
    [HttpPut("{locationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateLocation(
        [Description("ID локации.")] Guid locationId,
        [FromBody] UpdateLocationCommand command,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Загрузить фотографию локации.")]
    [HttpPut("{locationId:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult AddLocationImage(
        [Description("ID локации.")] Guid locationId,
        [Description("Фотография локации.")] IFormFile file,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Добавить администратора локации.")]
    [HttpPut("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public IActionResult AddLocationAdmin(
        [Description("ID локации.")] Guid locationId,
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    ) => NoContent();

    [EndpointSummary("Удалить администратора локации.")]
    [HttpDelete("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public IActionResult DeleteLocationAdmin(
        [Description("ID локации.")] Guid locationId,
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    ) => NoContent();

    [EndpointSummary("Удалить фотографию локации.")]
    [HttpDelete("{locationId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult DeleteLocationImage(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    ) => NoContent();

    [EndpointSummary("Удалить локацию.")]
    [HttpDelete("{locationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteLocation(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    ) => NoContent();
}
