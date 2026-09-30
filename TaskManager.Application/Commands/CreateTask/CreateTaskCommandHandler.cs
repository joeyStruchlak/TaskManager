using MediatR;                           // MediatR library — gives us IRequestHandler
using TaskManager.Application.DTOs;      // gives us TaskDto
using TaskManager.Domain.Entities;       // gives us TaskItem
using TaskManager.Domain.Interfaces;     // gives us ITaskRepository

namespace TaskManager.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
// "I am a handler. I handle CreateTaskCommand. I return a TaskDto when done."
{
    private readonly ITaskRepository _taskRepository;
    // a slot on this class to hold the database contract permanently

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    // constructor — when this class is created, ASP.NET gives us a repository automatically
    {
        _taskRepository = taskRepository;
        // save what was handed in into the permanent slot above
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    // MediatR calls this method when a CreateTaskCommand is sent
    // "request" IS the command — it has request.Title and request.Description on it
    {
        var task = new TaskItem(request.Title, request.Description);

        var createdTask = await _taskRepository.AddAsync(task);
        // save the task to the database and wait for it to finish
        // the database fills in the Id automatically
        // createdTask now has all 6 properties including the new Id

        return new TaskDto
        // build the response object to send back to whoever called this
        {
            Id = createdTask.Id,                    // the database-generated number
            Title = createdTask.Title,              // what the user typed
            Description = createdTask.Description,  // what the user typed
            IsCompleted = createdTask.IsCompleted,  // false
            CreatedAt = createdTask.CreatedAt,      // the timestamp we set above
            CompletedAt = createdTask.CompletedAt   // null — task just created
        };
    }
}