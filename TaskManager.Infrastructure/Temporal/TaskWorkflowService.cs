using Temporalio.Client;
using Temporalio.Workflows;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Temporal.Workflows;

namespace TaskManager.Infrastructure.Temporal;

public class TaskWorkflowService : ITaskWorkflowService
{
    private readonly ITemporalClient _temporalClient;

    public TaskWorkflowService(ITemporalClient temporalClient)
    {
        _temporalClient = temporalClient;
    }

    public async Task StartAssignmentWorkflowAsync(int taskId)
    {
        await _temporalClient.StartWorkflowAsync(
            (TaskAssignmentWorkflow wf) => wf.RunAsync(new TaskAssignmentWorkflowInput(taskId)),
            new WorkflowOptions
            {
                Id = $"task-assignment-{taskId}",
                TaskQueue = "task-manager"
            });
    }
}