namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Commands;
using Application.Contracts.Dto;
using Application.Contracts.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("resources")]
public class ResourcesController : ControllerBase
{
    [EndpointSummary("Получить список ресурсов.")]
    [HttpGet]
    [ProducesResponseType<ResourceDto[]>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult GetResources([FromQuery] GetResourcesQuery query, CancellationToken cancellationToken) => Ok();

    [EndpointSummary("Получить ресурс по ID.")]
    [HttpGet("{resourceId:guid}")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetResource(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Создать ресурс.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult CreateResource(
        [FromBody] CreateResourceCommand command,
        CancellationToken cancellationToken
    ) => CreatedAtAction(nameof(CreateResource), null);

    [EndpointSummary("Обновить ресурс.")]
    [HttpPut("{resourceId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateResource(
        [Description("ID ресурса.")] Guid resourceId,
        [FromBody] UpdateResourceCommand command,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Загрузить фотографию ресурса.")]
    [HttpPut("{resourceId:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType<string>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult AddResourceImage(
        [Description("ID ресурса.")] Guid resourceId,
        [Description("Фотография ресурса.")] IFormFile file,
        CancellationToken cancellationToken
    ) => Ok();

    [EndpointSummary("Удалить фотографию ресурса.")]
    [HttpDelete("{resourceId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult DeleteResourceImage(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    ) => NoContent();

    [EndpointSummary("Удалить ресурс.")]
    [HttpDelete("{resourceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteResource(
        [Description("ID ресурса.")] Guid resourceId,
        CancellationToken cancellationToken
    ) => NoContent();
}
