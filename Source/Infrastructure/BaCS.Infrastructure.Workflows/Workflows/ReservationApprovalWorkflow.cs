namespace BaCS.Infrastructure.Workflows.Workflows;

using Activities;
using Elsa.Extensions;
using Elsa.Scheduling.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Models;

public sealed class ReservationApprovalWorkflow : WorkflowBase
{
    public const string WorkflowDefinitionId = nameof(ReservationApprovalWorkflow);

    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Name = "Reservation Approval Workflow";
        builder.Description = "Workflow для обработки подтверждения резервации администратором";

        var reservationIdVariable = builder.WithVariable<Guid>().WithName(Variables.ReservationId);
        var locationHasAdminsVariable = builder.WithVariable<bool>().WithName(Variables.HasAdmins);

        var setLocationHasAdminsActivity = new SetLocationHasAdminsActivity
        {
            LocationHasAdmins = new Output<bool>(locationHasAdminsVariable)
        };

        var workflow = new Sequence
        {
            Activities =
            {
                new SetVariable
                {
                    Variable = reservationIdVariable,
                    Value = new Input<object>(context => context.GetInput<Guid>(Variables.ReservationId))
                },
                setLocationHasAdminsActivity,
                new If(context => !setLocationHasAdminsActivity.GetOutput<bool>(
                        context,
                        nameof(setLocationHasAdminsActivity.LocationHasAdmins)
                    )
                )
                {
                    Then = new End()
                },
                new SendPendingApprovalEmailActivity
                {
                    ReservationId = new Input<Guid>(reservationIdVariable)
                },
                new Delay(TimeSpan.FromSeconds(5)),
                new Parallel
                {
                    Activities =
                    {
                        new AutoApproveActivity
                        {
                            ReservationId = new Input<Guid>(reservationIdVariable)
                        },
                        new SendReservationApprovedEmailActivity
                        {
                            ReservationId = new Input<Guid>(reservationIdVariable)
                        }
                    }
                }
            }
        };

        builder.Root = workflow;
    }

    public static class Variables
    {
        public const string ReservationId = "ReservationId";
        public const string LocationId = "LocationId";
        public const string HasAdmins = "HasAdmins";
    }
}
