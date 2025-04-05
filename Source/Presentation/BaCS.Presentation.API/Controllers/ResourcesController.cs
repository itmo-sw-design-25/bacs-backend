namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Dto;
using Application.Contracts.Requests;
using Application.Contracts.Responces;
using Application.Handlers.Resources.Commands;
using Application.Handlers.Resources.Queries;
using Domain.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("resources")]
public class ResourcesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [EndpointSummary("Получить список ресурсов.")]
    [HttpGet]
    [ProducesResponseType<PaginatedResponse<ResourceDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> GetResources(
        [FromQuery] GetResourcesRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new GetResourcesQuery.Query(
            request.Ids,
            request.LocationIds,
            request.Types,
            request.Offset,
            request.Limit
        );
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [AllowAnonymous]
    [EndpointSummary("Получить ресурс по ID.")]
    [HttpGet("{resourceId:guid}")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> GetResource(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetResourceQuery.Query(resourceId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Создать ресурс.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> CreateResource(
        [FromBody] CreateResourceRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateResourceCommand.Command(
            request.LocationId,
            request.Name,
            request.Description,
            request.Floor,
            request.Equipment,
            request.Type
        );
        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateResource), result);
    }

    [EndpointSummary("Обновить ресурс.")]
    [HttpPut("{resourceId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> UpdateResource(
        [Description("ID ресурса.")] Guid resourceId,
        [FromBody] UpdateResourceRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateResourceCommand.Command(
            resourceId,
            request.Name,
            request.Description,
            request.Floor,
            request.Equipment,
            request.Type
        );
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Загрузить фотографию ресурса.")]
    [HttpPut("{resourceId:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> AddResourceImage(
        [Description("ID ресурса.")] Guid resourceId,
        [Description("Фотография ресурса.")] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        var command = new AddResourceImageCommand.Command(
            resourceId,
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

    [EndpointSummary("Удалить фотографию ресурса.")]
    [HttpDelete("{resourceId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> DeleteResourceImage(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteResourceImageCommand.Command(resourceId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [EndpointSummary("Удалить ресурс.")]
    [HttpDelete("{resourceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> DeleteResource(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    )
    {
        var command = new DeleteResourceCommand.Command(resourceId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
