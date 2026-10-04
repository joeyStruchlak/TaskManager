using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITimesheetRepository
    {
        Task<Timesheet?> GetByIdAsync(int id);
        Task<IEnumerable<Timesheet>> GetBySubmitterAsync(string submittedBy);
        Task<Timesheet> AddAsync(Timesheet timesheet);
        Task UpdateAsync(Timesheet timesheet);
    }
}