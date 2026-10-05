using Temporalio.Activities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Temporal.Activities;

// TEMPORAL CONCEPT: Activities
// This is where REAL WORK happens.
// Database calls, emails, external APIs - all go here.
// Temporal automatically retries these if they fail.
// Each method = one step in the workflow.
public class TaskAssignmentActivities
{
    private readonly ITaskRepository _taskRepository;

    // ITaskRepository is injected by .NET DI - same as anywhere else in your app
    public TaskAssignmentActivities(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    // TEMPORAL CONCEPT: Activity Method
    // [Activity] marks this as a single executable step.
    // Temporal calls this, handles retries, tracks success/failure.
    // STEP 1: Notify the assignee
    [Activity]
    public async Task SendAssignmentNotificationAsync(int taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null)
            throw new ApplicationException($"Task {taskId} not found.");

        // TODO: plug in real email service here (SendGrid, SMTP, etc.)
        // For now we log - this is where McMahon's notification system connects
        Console.WriteLine($"[Temporal] Notifying '{task.AssignedTo}' about task: {task.Title}");
    }

    // STEP 2: Check acknowledgement and escalate if needed
    [Activity]
    public async Task CheckAndEscalateIfNeededAsync(int taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task is null)
            throw new ApplicationException($"Task {taskId} not found.");

        // TODO: check acknowledgement flag on task, escalate to manager
        // For now we log the escalation check
        Console.WriteLine($"[Temporal] Checking acknowledgement for task: {task.Title}");
    }
}