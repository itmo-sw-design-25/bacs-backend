namespace BaCS.Presentation.API.Controllers;

using Application.Contracts.Requests;
using Application.Handlers.Reports.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("reports")]
public class ReportsController(IMediator mediator) : ControllerBase
{
    private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [EndpointSummary("Получить отчет по бронированиям за указанный период в Excel формате.")]
    [HttpPost("reservations")]
    [ProducesResponseType<FileStreamResult>(StatusCodes.Status200OK, XlsxContentType)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> GetReservationsReport(
        [FromQuery] GetReservationsReportRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var command = new ComposeReservationsReportCommand.Command(
            request.From,
            request.To,
            request.UserId,
            request.ResourceId,
            request.LocationId
        );

        var result = await mediator.Send(command, cancellationToken);

        return new FileStreamResult(result, XlsxContentType)
        {
            FileDownloadName = $"reservations_report_{request.From:O}_to_{request.To:O}.xlsx"
        };
    }
}
