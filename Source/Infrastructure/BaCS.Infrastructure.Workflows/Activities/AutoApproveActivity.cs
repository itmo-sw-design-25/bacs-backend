namespace BaCS.Infrastructure.Workflows.Activities;

using Application.Abstractions.Persistence;
using Application.Contracts.Exceptions;
using Domain.Core.Enums;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.EntityFrameworkCore;
using static Workflows.ReservationApprovalWorkflow;

[Activity("BaCS", "Reservation", "Auto approve reservation")]
public class AutoApproveActivity : CodeActivity
{
    [Input(Description = "Reservation ID")]
    public Input<Guid> ReservationId { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var dbContext = context.GetRequiredService<IBaCSDbContext>();

        var reservationId = ReservationId.GetOrDefault(context);

        if (reservationId == Guid.Empty)
        {
            context.AddExecutionLogEntry("Error", $"Input {Variables.ReservationId} was not found");
            context.Fault(
                new BusinessRulesException(
                    $"{Variables.ReservationId} input to execute activity {nameof(AutoApproveActivity)} was not found"
                )
            );

            return;
        }

        var reservation = await dbContext
            .Reservations
            .Include(r => r.User)
            .Include(r => r.Location)
            .Include(r => r.Resource)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reservationId, context.CancellationToken);

        if (reservation is null)
        {
            context.AddExecutionLogEntry("Error", $"Reservation {reservationId} not found");
            context.Fault(new BusinessRulesException($"Reservation {reservationId} not found to auto-approve"));

            return;
        }

        if (reservation.Status != ReservationStatus.PendingApproval)
        {
            context.AddExecutionLogEntry(
                "Warning",
                $"Reservation {reservationId} is in status {reservation.Status}, skipping"
            );

            return;
        }

        await dbContext.Reservations.ExecuteUpdateAsync(
            x => x.SetProperty(r => r.Status, ReservationStatus.Accepted),
            cancellationToken: context.CancellationToken
        );

        await dbContext.SaveChangesAsync(context.CancellationToken);

        context.AddExecutionLogEntry("Info", $"Reservation {reservationId} has been successfully auto-approved");
    }
}
