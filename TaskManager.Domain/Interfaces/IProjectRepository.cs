using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task AddAsync(Project project);
    Task<Project?> GetByIdAsync(int id);
    Task UpdateAsync(Project project);
}