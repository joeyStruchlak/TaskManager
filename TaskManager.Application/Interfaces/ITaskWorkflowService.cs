namespace TaskManager.Application.Interfaces;

public interface ITaskWorkflowService
{
    Task StartAssignmentWorkflowAsync(int taskId);
}