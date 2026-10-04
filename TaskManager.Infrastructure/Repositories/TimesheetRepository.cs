using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly TaskDbContext _context;

        public TimesheetRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<Timesheet?> GetByIdAsync(int id)
        {
            return await _context.Timesheets.FindAsync(id);
        }

        public async Task<IEnumerable<Timesheet>> GetBySubmitterAsync(string submittedBy)
        {
            return await _context.Timesheets
                .Where(t => t.SubmittedBy == submittedBy)
                .ToListAsync();
        }

        public async Task<Timesheet> AddAsync(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            await _context.SaveChangesAsync();
            return timesheet;
        }

        public async Task UpdateAsync(Timesheet timesheet)
        {
            _context.Timesheets.Update(timesheet);
            await _context.SaveChangesAsync();
        }
    }
}