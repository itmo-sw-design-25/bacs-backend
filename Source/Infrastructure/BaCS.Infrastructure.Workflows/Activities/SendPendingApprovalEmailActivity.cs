namespace BaCS.Infrastructure.Workflows.Activities;

using Application.Abstractions.Integrations;
using Application.Abstractions.Persistence;
using Application.Contracts.Exceptions;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.EntityFrameworkCore;
using static Workflows.ReservationApprovalWorkflow;

[Activity("BaCS", "Reservation", "Send pending approval notification to admins")]
public class SendPendingApprovalEmailActivity : CodeActivity
{
    [Input(Description = "Reservation ID")]
    public Input<Guid> ReservationId { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var dbContext = context.GetRequiredService<IBaCSDbContext>();
        var emailNotifier = context.GetRequiredService<IEmailNotifier>();

        var reservationId = ReservationId.GetOrDefault(context);

        if (reservationId == Guid.Empty)
        {
            context.AddExecutionLogEntry("Error", $"Input {Variables.ReservationId} was not found");
            context.Fault(
                new BusinessRulesException(
                    $"{Variables.ReservationId} input to execute activity {nameof(SendPendingApprovalEmailActivity)} was not found"
                )
            );

            return;
        }

        var reservation = await dbContext
            .Reservations
            .Include(r => r.User)
            .Include(r => r.Location)
            .ThenInclude(l => l.Admins)
            .Include(r => r.Resource)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reservationId, context.CancellationToken);

        if (reservation is null) return;

        await emailNotifier.SendReservationPendingApprovalToAdmins(
            reservation,
            reservation.Location,
            reservation.Resource,
            reservation.User,
            reservation.Location.Admins,
            context.CancellationToken
        );

        context.AddExecutionLogEntry(
            "Info",
            $"Pending approval emails for reservation {reservationId} have been successfully sent to admins"
        );
    }
}
