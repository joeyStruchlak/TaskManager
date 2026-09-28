// --- IMPORTS: Bringing in the classes and namespaces we need ---
using TaskManager.Application.DTOs;      // Gives access to TaskDto
using TaskManager.Domain.Entities;      // Gives access to TaskItem
using TaskManager.Domain.Interfaces;    // Gives access to the database contract ITaskRepository
using System.Threading.Tasks;           // Enables asynchronous programming tools

// --- NAMESPACE: Folder location for Use Cases ---
namespace TaskManager.Application.UseCases;

/// <summary>
/// USE CASE: Handles fetching all tasks from the system and preparing them for display.
/// </summary>
public class GetAllTasksUseCase
{
    // 1. Private variable to hold our database contract.
    // 'readonly' means it can only be set inside the constructor.
    private readonly ITaskRepository _repository;

    // 2. CONSTRUCTOR: Asks for a database implementation (repository) when created.
    public GetAllTasksUseCase(ITaskRepository repository)
    {
        _repository = repository; // Save the repository instance locally
    }

    // 3. EXECUTE METHOD: The main function that runs when someone requests all tasks.
    // 'async Task<IEnumerable<TaskDto>>' means:
    // "Runs asynchronously and eventually returns a list (IEnumerable) of TaskDto items."
    public async Task<IEnumerable<TaskDto>> ExecuteAsync()
    {
        // STEP A: Go to the database through the contract and fetch raw TaskItem entities.
        // 'await' tells C# to wait for the database query to complete without freezing the app.
        var tasks = await _repository.GetAllAsync();

        // STEP B: Convert (Map) each raw 'TaskItem' entity into a clean 'TaskDto' output object.
        // '.Select()' is C#'s way of saying "Loop through every item in this list and transform it".
        return tasks.Select(task => new TaskDto
        {
            Id = task.Id,                       // Copy the database ID
            Title = task.Title,                 // Copy the title
            Description = task.Description,     // Copy the description
            IsCompleted = task.IsCompleted,     // Copy completion status
            CreatedAt = task.CreatedAt,         // Copy creation timestamp
            CompletedAt = task.CompletedAt      // Copy completion timestamp (if any)
        });
    }
}