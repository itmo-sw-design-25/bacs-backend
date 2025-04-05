namespace BaCS.Presentation.API.Controllers;

using System.ComponentModel;
using Application.Contracts.Dto;
using Application.Contracts.Requests;
using Application.Contracts.Responses;
using Application.Handlers.Reservations.Commands;
using Application.Handlers.Reservations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("reservations")]
public class ReservationsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [EndpointSummary("Получить список резерваций.")]
    [HttpGet]
    [ProducesResponseType<PaginatedResponse<ReservationDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> GetReservations(
        [FromQuery] GetReservationsRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new GetReservationsQuery.Query(
            request.Ids,
            request.UserIds,
            request.ResourceIds,
            request.LocationIds,
            request.Statuses,
            request.BeforeDate,
            request.AfterDate,
            request.Offset,
            request.Limit
        );
        var reservations = await mediator.Send(query, cancellationToken);

        return Ok(reservations);
    }

    [AllowAnonymous]
    [EndpointSummary("Получить резервацию по ID.")]
    [HttpGet("{reservationId:guid}")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> GetReservation(
        [Description("ID резервации.")] Guid reservationId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetReservationQuery.Query(reservationId);
        var reservation = await mediator.Send(query, cancellationToken);

        return Ok(reservation);
    }

    [EndpointSummary("Создать резервацию.")]
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> CreateReservation(
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new CreateReservationCommand.Command(
            request.ResourceId,
            request.LocationId,
            request.From,
            request.To
        );
        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(CreateReservation), result);
    }

    [EndpointSummary("Обновить резервацию.")]
    [HttpPut("{reservationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> UpdateReservation(
        [Description("ID резервации.")] Guid reservationId,
        [FromBody] UpdateReservationRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateReservationCommand.Command(reservationId, request.From, request.To);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [EndpointSummary("Отменить резервацию.")]
    [HttpPut("{reservationId:guid}/Cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden, "application/problem+json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")]
    public async Task<IActionResult> CancelReservation(
        [Description("ID резервации.")] Guid reservationId,
        CancellationToken cancellationToken
    )
    {
        var command = new CancelReservationCommand.Command(reservationId);
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
