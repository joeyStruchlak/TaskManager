// --- IMPORTS (Bringing in tools from other projects) ---
using TaskManager.Application.DTOs;      // Brings in simple data objects (DTOs) used for sending data over the web
using TaskManager.Domain.Entities;      // Brings in your core business objects (like TaskItem)
using TaskManager.Domain.Interfaces;    // Brings in your database contracts (like ITaskRepository)

// --- NAMESPACE (The folder address of this file inside the project) ---
namespace TaskManager.Application.UseCases;

/// <summary>
/// This class handles ONE specific job: Creating a task when a user clicks "Save".
/// </summary>
public class CreateTaskUseCase
{
    // 1. A private placeholder variable to hold our database contract.
    // 'readonly' means it cannot be changed after this class is created.
    private readonly ITaskRepository _repository;

    // 2. CONSTRUCTOR: When this Use Case starts up, it asks for a database implementation.
    // We pass in the repository contract so this class can talk to the database.
    public CreateTaskUseCase(ITaskRepository repository)
    {
        _repository = repository; // Store the repository into our private variable above
    }

    // 3. EXECUTE METHOD: The main function that runs when the user submits a new task.
    // 'async Task<TaskDto>' means: "This runs asynchronously, and when finished, hands back a TaskDto."
    // 'CreateTaskRequest request' is the raw form data sent in by the user (Title, Description).
    public async System.Threading.Tasks.Task<TaskDto> ExecuteAsync(CreateTaskRequest request)
    {
        // STEP A: Create a brand new Domain Entity (TaskItem) using the user's input.
        var task = new TaskItem
        {
            Title = request.Title,              // Copy the Title the user typed
            Description = request.Description,  // Copy the Description the user typed
            IsCompleted = false,                // New tasks always start as not completed
            CreatedAt = DateTime.UtcNow         // Record the exact time this task was created (in global standard time)
        };

        // STEP B: Save the new task into the database.
        // 'await' tells C# to pause and wait for the database to finish saving before moving to the next line.
        var createdTask = await _repository.AddAsync(task);

        // STEP C: Convert (Map) the saved database item into a clean DTO summary to send back to the web client.
        return new TaskDto
        {
            Id = createdTask.Id,                       // The database auto-generated ID (e.g., 1, 2, 3...)
            Title = createdTask.Title,                 // The saved title
            Description = createdTask.Description,     // The saved description
            IsCompleted = createdTask.IsCompleted,     // false
            CreatedAt = createdTask.CreatedAt,         // The timestamp created above
            CompletedAt = createdTask.CompletedAt      // Null (since it's brand new)
        };
    }
}