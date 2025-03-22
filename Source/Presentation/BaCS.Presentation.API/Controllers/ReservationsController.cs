namespace BaCS.Presentation.API.Controllers;

using Application.Contracts.Commands;
using Application.Contracts.Dto;
using Application.Contracts.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    [EndpointSummary("Получить список резерваций.")]
    [HttpGet]
    [ProducesResponseType<ReservationDto[]>(StatusCodes.Status200OK, "application/json")]
    public IActionResult GetReservations([FromQuery] GetReservationsQuery query) => Ok();

    [EndpointSummary("Получить резервацию по ID.")]
    [HttpGet("{reservationId:guid}")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult GetReservation([FromRoute] Guid reservationId) => Ok();

    [EndpointSummary("Создать резервацию.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public IActionResult CreateReservation([FromBody] CreateReservationCommand command) =>
        CreatedAtAction(nameof(CreateReservation), null);

    [EndpointSummary("Обновить резервацию.")]
    [HttpPut("{reservationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult UpdateReservation([FromRoute] Guid reservationId, [FromBody] UpdateReservationCommand command) => Ok();

    [EndpointSummary("Отменить резервацию.")]
    [HttpPut("{reservationId:guid}/Cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public IActionResult CancelReservation([FromRoute] Guid reservationId) => NoContent();
}
