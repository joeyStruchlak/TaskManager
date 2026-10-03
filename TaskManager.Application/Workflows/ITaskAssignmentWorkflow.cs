using Temporalio.Workflows;

namespace TaskManager.Application.Workflows;

// TEMPORAL CONCEPT: Workflow Contract
// This is the CONTRACT - it defines what the workflow can do.
// It lives in Application because Application owns contracts.
// Infrastructure will provide the actual implementation.
// Nothing in here runs - this is just the definition.

[Workflow] // Tells Temporal: "this is a workflow definition"
public interface ITaskAssignmentWorkflow
{
    // TEMPORAL CONCEPT: Entry Point
    // When someone starts this workflow, Temporal calls this method.
    // It's the front door. Everything begins here.
    // Triggered by: CreateTaskCommandHandler (in Application layer)
    // after a task is saved to the database.
    [WorkflowRun]
    Task RunAsync(TaskAssignmentWorkflowInput input);
}

// TEMPORAL CONCEPT: Workflow Input
// This is the data bag passed into the workflow when it starts.
// We only pass the ID - the workflow will look up the full task
// inside an Activity (which has database access).
// Temporal serializes this to JSON and stores it durably.
public record TaskAssignmentWorkflowInput(int TaskId);