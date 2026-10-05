namespace TaskManager.Application.Interfaces;

public interface ITimesheetWorkflowService
{
    Task StartApprovalWorkflowAsync(int timesheetId);
}