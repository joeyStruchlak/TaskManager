using Temporalio.Activities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Activities
{
    public class TimesheetApprovalActivities
    {
        private readonly ITimesheetRepository _repository;

        public TimesheetApprovalActivities(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        [Activity]
        public async Task NotifySupervisorAsync(int timesheetId)
        {
            var timesheet = await _repository.GetByIdAsync(timesheetId);
            if (timesheet == null) return;
            Console.WriteLine($"Supervisor notified for timesheet {timesheetId} — {timesheet.SupervisorEmail}");
        }

        [Activity]
        public async Task<string> CheckApprovalStatusAsync(int timesheetId)
        {
            var timesheet = await _repository.GetByIdAsync(timesheetId);
            return timesheet?.Status ?? "NotFound";
        }

        [Activity]
        public async Task EscalateToPayrollManagerAsync(int timesheetId)
        {
            var timesheet = await _repository.GetByIdAsync(timesheetId);
            if (timesheet == null) return;
            Console.WriteLine($"Timesheet {timesheetId} escalated — supervisor {timesheet.SupervisorEmail} did not respond.");
        }
    }
}