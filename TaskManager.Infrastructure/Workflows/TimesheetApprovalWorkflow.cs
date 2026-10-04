using Temporalio.Workflows;
using TaskManager.Infrastructure.Activities;

namespace TaskManager.Infrastructure.Workflows
{
    [Workflow]
    public class TimesheetApprovalWorkflow
    {
        [WorkflowRun]
        public async Task RunAsync(int timesheetId)
        {
            // Step 1: Notify the supervisor
            await Workflow.ExecuteActivityAsync(
                (TimesheetApprovalActivities a) => a.NotifySupervisorAsync(timesheetId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
            );

            // Step 2: Wait 48 hours for supervisor to act
            await Workflow.DelayAsync(TimeSpan.FromHours(48));

            // Step 3: Check if supervisor responded
            var status = await Workflow.ExecuteActivityAsync(
                (TimesheetApprovalActivities a) => a.CheckApprovalStatusAsync(timesheetId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
            );

            // Step 4: Still pending after 48 hours — escalate
            if (status == "Pending")
            {
                await Workflow.ExecuteActivityAsync(
                    (TimesheetApprovalActivities a) => a.EscalateToPayrollManagerAsync(timesheetId),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
                );
            }
        }
    }
}