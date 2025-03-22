namespace BaCS.Presentation.API.Controllers;

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
    public IActionResult GetResources([FromQuery] GetResourcesQuery query) => Ok();

    [EndpointSummary("Получить ресурс по ID.")]
    [HttpGet("{resourceId:guid}")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetResource([FromRoute] Guid resourceId) => Ok();

    [EndpointSummary("Создать ресурс.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult CreateResource([FromBody] CreateResourceCommand command) =>
        CreatedAtAction(nameof(CreateResource), null);

    [EndpointSummary("Обновить ресурс.")]
    [HttpPut("{resourceId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<ResourceDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult UpdateResource([FromRoute] Guid resourceId, [FromBody] UpdateResourceCommand command) => Ok();

    [EndpointSummary("Удалить ресурс.")]
    [HttpDelete("{resourceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult DeleteResource([FromRoute] Guid resourceId) => NoContent();
}
