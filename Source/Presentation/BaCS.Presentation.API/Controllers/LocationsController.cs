namespace BaCS.Presentation.API.Controllers;

using Application.Contracts.Commands;
using Application.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    [EndpointSummary("Получить список локаций.")]
    [HttpGet]
    [ProducesResponseType<LocationDto[]>(StatusCodes.Status200OK, "application/json")]
    public IActionResult GetLocations() => Ok();

    [EndpointSummary("Получить локацию по ID.")]
    [HttpGet("{locationId:guid}")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetLocation([FromRoute] Guid locationId) => Ok();

    [EndpointSummary("Создать локацию.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult CreateLocation([FromBody] CreateLocationCommand command) =>
        CreatedAtAction(nameof(CreateLocation), null);

    [EndpointSummary("Обновить локацию.")]
    [HttpPut("{locationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateLocation([FromRoute] Guid locationId, [FromBody] UpdateLocationCommand command) => Ok();

    [EndpointSummary("Загрузить фотографию локации.")]
    [HttpPut("{locationId:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<string>(StatusCodes.Status204NoContent, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult AddLocationImage([FromRoute] Guid locationId, IFormFile file) => Ok();

    [EndpointSummary("Добавить администратора локации.")]
    [HttpPut("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public IActionResult AddLocationAdmin([FromRoute] Guid locationId, [FromRoute] Guid userId) => NoContent();

    [EndpointSummary("Удалить администратора локации.")]
    [HttpDelete("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public IActionResult DeleteLocationAdmin([FromRoute] Guid locationId, [FromRoute] Guid userId) => NoContent();

    [EndpointSummary("Удалить фотографию локации.")]
    [HttpDelete("{locationId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult DeleteLocationImage([FromRoute] Guid locationId) => NoContent();

    [EndpointSummary("Удалить локацию.")]
    [HttpDelete("{locationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteLocation([FromRoute] Guid locationId) => NoContent();
}
