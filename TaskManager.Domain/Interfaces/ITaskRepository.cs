// --- IMPORTS: Bringing in the TaskItem entity so the interface knows what object it works with ---
using TaskManager.Domain.Entities;

// --- NAMESPACE: Folder address for interfaces ---
namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Repository Interface: A contract that lists what database operations our app needs.
/// It contains NO code logic—only method signatures.
/// </summary>
public interface ITaskRepository
{
    // CONTRACT ITEM 1: "Whoever implements this must provide a way to fetch all tasks asynchronously."
    // Returns a collection (IEnumerable) of TaskItem objects.
    Task<IEnumerable<TaskItem>> GetAllAsync();

    // CONTRACT ITEM 2: "Whoever implements this must provide a way to save a new task."
    // Takes a TaskItem as input, saves it, and returns the saved TaskItem (with its new ID).
    Task<TaskItem> AddAsync(TaskItem task);
}