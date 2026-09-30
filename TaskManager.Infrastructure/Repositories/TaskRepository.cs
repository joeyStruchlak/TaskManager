// --- IMPORTS ---
using Microsoft.EntityFrameworkCore;         // Gives us SQL query tools like ToListAsync()
using TaskManager.Domain.Entities;           // Gives us TaskItem
using TaskManager.Domain.Interfaces;         // Gives us the ITaskRepository contract to fulfill
using TaskManager.Infrastructure.Data;       // Gives us TaskDbContext

namespace TaskManager.Infrastructure.Repositories;

/// <summary>
/// This class fulfills the contract ('ITaskRepository') defined in the Domain layer.
/// It contains the actual Entity Framework C# code that queries and updates the database.
/// </summary>
public class TaskRepository : ITaskRepository
{
    // 1. Private variable to hold our active database connection context.
    private readonly TaskDbContext _context;

    // 2. CONSTRUCTOR: Receives the database context so this class can execute SQL queries.
    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    // 3. GET ALL TASKS METHOD
    public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        // SQL Equivalent: SELECT * FROM Tasks ORDER BY CreatedAt DESC;
        return await _context.Tasks
            .OrderByDescending(t => t.CreatedAt) // Sort newest tasks first
            .ToListAsync();                      // Run query asynchronously and hand back a list
    }

    // 4. ADD TASK METHOD
    public async System.Threading.Tasks.Task<TaskItem> AddAsync(TaskItem task)
    {
        // Stage the new task in EF Core memory (does NOT write to SQL yet)
        _context.Tasks.Add(task);

        // SQL Equivalent: INSERT INTO Tasks ... 
        // Actually sends the INSERT command to SQL and assigns the generated Id back to 'task'
        await _context.SaveChangesAsync();

        // Return the saved task (now containing its newly assigned database Id)
        return task;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }


}