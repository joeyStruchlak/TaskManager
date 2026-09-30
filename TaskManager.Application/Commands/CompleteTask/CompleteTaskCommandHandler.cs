using MediatR;                          // MediatR library — gives us IRequestHandler
using TaskManager.Domain.Interfaces;    // Gives us ITaskRepository

namespace TaskManager.Application.Commands.CompleteTask;

// This class IS the handler — MediatR finds it automatically when CompleteTaskCommand is sent
// IRequestHandler<CompleteTaskCommand, bool> means: "I handle CompleteTaskCommand and return bool"
public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, bool>
{
    // FIELD — the tool this class needs to talk to the database
    // private = only this class can use it
    // readonly = once set in constructor, never changes
    // underscore prefix = convention for private fields in C#
    private readonly ITaskRepository _taskRepository;

    // CONSTRUCTOR — runs once when this handler is first created
    // .NET reads this and says "it needs an ITaskRepository, I'll provide one automatically"
    // That automatic providing is called Dependency Injection
    public CompleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        // Store the provided repository so the Handle method can use it later
        _taskRepository = taskRepository;
    }

    // HANDLE METHOD — MediatR calls this when someone sends CompleteTaskCommand
    // request = the command itself, contains request.TaskId
    // cancellationToken = lets the caller cancel the operation if needed (e.g. browser tab closed)
    public async Task<bool> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        // Go to the database and find the task with this id
        // Returns null if it doesn't exist (that's what the ? means on TaskItem?)
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        // EXCEPTION PATH — if task doesn't exist, throw a clear error
        // KeyNotFoundException is a built-in .NET exception for "id not found"
        if (task is null)
            throw new KeyNotFoundException($"Task {request.TaskId} not found.");

        // DDD — business logic lives ON the entity, not here
        // MarkComplete() sets IsCompleted = true and CompletedAt = now
        task.MarkComplete();

        // Save the changes back to the database
        await _taskRepository.UpdateAsync(task);

        // Tell the caller it worked
        return true;
    }
}