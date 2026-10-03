using Temporalio.Workflows;
using Temporalio.Common;
using TaskManager.Application.Workflows;
using TaskManager.Infrastructure.Activities;

namespace TaskManager.Infrastructure.Workflows;

// TEMPORAL CONCEPT: Workflow Implementation
// This is the BRAIN of the workflow - the orchestrator.
// It defines the sequence of steps but does NO real work itself.
// No database calls. No emails. No external APIs. Ever.
// All real work is delegated to Activities.
[Workflow]
public class TaskAssignmentWorkflow : ITaskAssignmentWorkflow
{
    // TEMPORAL CONCEPT: WorkflowRun Entry Point
    // Temporal calls this method when the workflow starts.
    // This is where we define the sequence of steps.
    [WorkflowRun]
    public async Task RunAsync(TaskAssignmentWorkflowInput input)
    {
        // TEMPORAL CONCEPT: Activity Options
        // How should Temporal handle failures in our activities?
        // StartToCloseTimeout = max time one activity execution can take
        // RetryPolicy = if it fails, try again. 3 times. Wait longer each time.
        var options = new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromMinutes(5),
            RetryPolicy = new RetryPolicy
            {
                MaximumAttempts = 3
            }
        };

        // STEP 1: Send assignment notification
        // We call the Activity - Temporal handles retries if it fails
        await Workflow.ExecuteActivityAsync(
            (TaskAssignmentActivities act) => act.SendAssignmentNotificationAsync(input.TaskId),
            options);

        // STEP 2: Wait 24 hours durably
        // Server can crash here. Temporal will wake this back up.
        await Workflow.DelayAsync(TimeSpan.FromHours(24));

        // STEP 3: Check if task was acknowledged
        await Workflow.ExecuteActivityAsync(
            (TaskAssignmentActivities act) => act.CheckAndEscalateIfNeededAsync(input.TaskId),
            options);
    }
}