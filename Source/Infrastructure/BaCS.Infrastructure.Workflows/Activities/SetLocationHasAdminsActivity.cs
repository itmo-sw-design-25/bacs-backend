namespace BaCS.Infrastructure.Workflows.Activities;

using Application.Abstractions.Persistence;
using Application.Contracts.Exceptions;
using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.EntityFrameworkCore;
using static Workflows.ReservationApprovalWorkflow;

[Activity("BaCS", "Location", "Returns True, if location has admins and False otherwise")]
public class SetLocationHasAdminsActivity : CodeActivity
{
    [Output(Description = "Whether location has admins")]
    public Output<bool> LocationHasAdmins { get; set; } = default!;

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var dbContext = context.GetRequiredService<IBaCSDbContext>();

        if (context.WorkflowInput.TryGetValue(Variables.LocationId, out Guid locationId) is false)
        {
            context.AddExecutionLogEntry("Error", $"Input {Variables.LocationId} was not found");
            context.Fault(
                new BusinessRulesException(
                    $"{Variables.LocationId} input to execute activity {nameof(SetLocationHasAdminsActivity)} was not found"
                )
            );

            return;
        }

        var hasAdmins = await dbContext.LocationAdmins.AnyAsync(
            x => x.LocationId == locationId,
            context.CancellationToken
        );

        context.Set(LocationHasAdmins, hasAdmins);
    }
}
