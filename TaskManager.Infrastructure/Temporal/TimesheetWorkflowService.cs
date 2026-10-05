using Temporalio.Client;
using Temporalio.Workflows;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Temporal.Workflows;

namespace TaskManager.Infrastructure.Temporal
{
    public class TimesheetWorkflowService : ITimesheetWorkflowService
    {
        private readonly ITemporalClient _temporalClient;

        public TimesheetWorkflowService(ITemporalClient temporalClient)
        {
            _temporalClient = temporalClient;
        }

        public async Task StartApprovalWorkflowAsync(int timesheetId)
        {
            await _temporalClient.StartWorkflowAsync(
                (TimesheetApprovalWorkflow wf) => wf.RunAsync(timesheetId),
                new WorkflowOptions
                {
                    Id = $"timesheet-approval-{timesheetId}",
                    TaskQueue = "task-manager"
                }
            );
        }
    }
}