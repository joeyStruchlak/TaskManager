using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Commands.Timesheets
{
    public class SubmitTimesheetCommandHandler : IRequestHandler<SubmitTimesheetCommand, int>
    {
        private readonly ITimesheetRepository _repository;
        private readonly ITimesheetWorkflowService _workflowService;

        public SubmitTimesheetCommandHandler(
            ITimesheetRepository repository,
            ITimesheetWorkflowService workflowService)
        {
            _repository = repository;
            _workflowService = workflowService;
        }

        public async Task<int> Handle(SubmitTimesheetCommand request, CancellationToken cancellationToken)
        {
            var timesheet = new Timesheet(
                request.SubmittedBy,
                request.SupervisorEmail,
                request.TotalHours
            );

            await _repository.AddAsync(timesheet);
            await _workflowService.StartApprovalWorkflowAsync(timesheet.Id);

            return timesheet.Id;
        }
    }
}