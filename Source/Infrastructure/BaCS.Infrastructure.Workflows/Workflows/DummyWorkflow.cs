namespace BaCS.Infrastructure.Workflows.Workflows;

using Elsa.Scheduling.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Activities;

public class DummyWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Name = "Dummy Workflow";
        builder.Description = "Workflow для тестирования интеграции с движком бизнес-процессов";

        var workflow = new Sequence
        {
            Activities =
            {
                new WriteLine("Hello World!"),
                new Delay(TimeSpan.FromSeconds(1)),
                new WriteLine("It is nice to meet you!")
            }
        };

        builder.Root = workflow;
    }
}
