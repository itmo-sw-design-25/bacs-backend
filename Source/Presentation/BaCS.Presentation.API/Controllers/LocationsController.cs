namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Dto;
using Application.Contracts.Requests;
using Application.Contracts.Responces;
using Application.Handlers.Commands.Locations;
using Application.Handlers.Queries.Locations;
using Domain.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("locations")]
public class LocationsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [EndpointSummary("Получить список локаций.")]
    [HttpGet]
    [ProducesResponseType<PaginatedResponse<LocationDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> GetLocations(
        [FromQuery] GetLocationsRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new GetLocationsQuery.Query(request.Ids, request.Skip, request.Take);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [EndpointSummary("Получить локацию по ID.")]
    [HttpGet("{locationId:guid}")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> GetLocation(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetLocationQuery.Query(locationId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Создать локацию.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> CreateLocation(
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateLocationCommand.Command(
            request.Name,
            request.Address,
            request.Description,
            request.CalendarSettings
        );

        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateLocation), result);
    }

    [EndpointSummary("Обновить локацию.")]
    [HttpPut("{locationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<LocationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> UpdateLocation(
        [FromRoute] Guid locationId,
        [FromBody] UpdateLocationRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateLocationCommand.Command(
            locationId,
            request.Name,
            request.Address,
            request.Description,
            request.CalendarSettings
        );

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Загрузить фотографию локации.")]
    [HttpPut("{locationId:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> AddLocationImage(
        [Description("ID локации.")] Guid locationId,
        [Description("Фотография локации.")] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        var command = new AddLocationImageCommand.Command(
            locationId,
            new ImageInfo
            {
                FileName = file.FileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                ImageData = file.OpenReadStream()
            }
        );

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Добавить администратора локации.")]
    [HttpPut("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public async Task<IActionResult> AddLocationAdmin(
        [Description("ID локации.")] Guid locationId,
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    )
    {
        var command = new AddLocationAdminCommand.Command(locationId, userId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [EndpointSummary("Удалить администратора локации.")]
    [HttpDelete("{locationId:guid}/admins/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    public async Task<IActionResult> DeleteLocationAdmin(
        [Description("ID локации.")] Guid locationId,
        [Description("ID пользователя.")] Guid userId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteLocationAdminCommand.Command(locationId, userId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [EndpointSummary("Удалить фотографию локации.")]
    [HttpDelete("{locationId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> DeleteLocationImage(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteLocationImageCommand.Command(locationId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [EndpointSummary("Удалить локацию.")]
    [HttpDelete("{locationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> DeleteLocation(
        [Description("ID локации.")] Guid locationId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteLocationCommand.Command(locationId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
