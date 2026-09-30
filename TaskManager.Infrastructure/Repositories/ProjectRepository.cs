using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    // FIELD: holds our database connection for use across all methods
    // private = only this class can touch it
    // readonly = can only be set once (in the constructor below) — never accidentally overwritten
    private readonly TaskDbContext _context;

    // CONSTRUCTOR: runs once when DI creates this class
    // ASP.NET sees "I need TaskDbContext" and injects it automatically
    // We store it in _context so every method below can use the same connection
    public ProjectRepository(TaskDbContext context)
    {
        _context = context;
    }

    // METHOD: reads ALL projects from the database
    // _context.Projects = the Projects table (DbSet we added to TaskDbContext)
    // ToListAsync() = EF Core runs SELECT * FROM Projects and returns a List
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    // METHOD: inserts ONE new project row into the database
    // AddAsync() = tells EF Core "track this new entity"
    // SaveChangesAsync() = actually runs the INSERT SQL — nothing saves without this
    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    // METHOD: reads ONE project by its Id
    // FindAsync() = SELECT * FROM Projects WHERE Id = @id
    // Returns Project? — the ? means "might be null if not found" (won't crash)
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    // METHOD: updates an existing project row
    // Update() = tells EF Core "this entity has changed, track it"
    // SaveChangesAsync() = runs the UPDATE SQL
    // Note: no AddAsync here — row already exists, we're changing it
    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }
}