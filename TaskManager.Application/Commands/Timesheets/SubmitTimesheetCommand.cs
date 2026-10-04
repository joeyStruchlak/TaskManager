using MediatR;


namespace TaskManager.Application.Commands.Timesheets
{
    public record SubmitTimesheetCommand(
        string SubmittedBy,
        string SupervisorEmail,
        decimal TotalHours
        ) : IRequest<int>;
}
