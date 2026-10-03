using MediatR;                           // MediatR library — gives us IRequestHandler
using TaskManager.Application.DTOs;      // gives us TaskDto
using TaskManager.Domain.Entities;       // gives us TaskItem
using TaskManager.Domain.Interfaces;     // gives us ITaskRepository
using TaskManager.Application.Workflows; // gives us ITaskAssignmentWorkflow
using Temporalio.Client;                 // gives us ITemporalClient

namespace TaskManager.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
// "I am a handler. I handle CreateTaskCommand. I return a TaskDto when done."
{
    // FIELD: A private readonly field is a variable that belongs to THIS class.
    // private = only this class can use it
    // readonly = it gets set once (in the constructor) and never changes
    // Think of it as a permanent slot that holds a tool this class needs to do its job.
    private readonly ITaskRepository _taskRepository;
    // slot 1: holds the database tool

    // TEMPORAL FIELD: Same pattern — a permanent slot for the Temporal client.
    // ITemporalClient is the tool that sends "start workflow" to the Temporal server.
    private readonly ITemporalClient _temporalClient;
    // slot 2: holds the Temporal connection tool

    // CONSTRUCTOR: This runs ONCE when .NET creates this class.
    // It's how .NET hands us our tools automatically (Dependency Injection).
    // You don't call this yourself — .NET calls it behind the scenes.
    // Every C# class can have one. It always has the same name as the class.
    public CreateTaskCommandHandler(ITaskRepository taskRepository, ITemporalClient temporalClient)
    {
        _taskRepository = taskRepository;
        // save the database tool into slot 1
        _temporalClient = temporalClient;
        // save the Temporal tool into slot 2
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    // MediatR calls this method when a CreateTaskCommand is sent
    // "request" IS the command — it has request.Title, request.Description etc on it
    {
        // STEP 1: Build the domain entity and save to database (unchanged)
        var task = new TaskItem(
            request.Title,
            request.Description,
            request.Priority,
            request.AssignedTo,
            request.DueDate);

        var createdTask = await _taskRepository.AddAsync(task);
        // save to database and wait — database fills in the Id automatically
        // createdTask now has all properties including the new Id

        // STEP 2: Tell Temporal to start the workflow
        // TEMPORAL CONCEPT: StartWorkflowAsync fires the workflow on the Temporal server.
        // The WorkflowId is the unique business key — Temporal guarantees no two workflows
        // with the same ID run at the same time. Prevents duplicates.
        // TaskQueue must match what the Worker is listening on — "task-manager"
        await _temporalClient.StartWorkflowAsync(
            (ITaskAssignmentWorkflow wf) => wf.RunAsync(new TaskAssignmentWorkflowInput(createdTask.Id)),
            new WorkflowOptions
            {
                Id = $"task-assignment-{createdTask.Id}",
                // e.g. "task-assignment-42" — unique per task
                TaskQueue = "task-manager"
                // must match the queue name in Program.cs
            });

        // STEP 3: Return the response to the API (unchanged)
        return new TaskDto
        // build the response object to send back to whoever called this
        {
            Id = createdTask.Id,                    // the database-generated number
            Title = createdTask.Title,              // what the user typed
            Description = createdTask.Description,  // what the user typed
            IsCompleted = createdTask.IsCompleted,  // false
            CreatedAt = createdTask.CreatedAt,      // the timestamp set automatically
            CompletedAt = createdTask.CompletedAt,
            Priority = createdTask.Priority,
            AssignedTo = createdTask.AssignedTo,
            DueDate = createdTask.DueDate
        };
    }
}

// Private readonly field — it's a field (not a property, not a variable in the traditional sense). A field belongs to the class. private means nothing outside this class can touch it. readonly means once it's set in the constructor it never changes. The underscore prefix _taskRepository is a naming convention that means "this is a field, not a local variable."

// Constructor — yes, every C# class can have one. It runs once when the class is created. Its job is to receive the tools the class needs (via Dependency Injection) and store them in the fields. .NET calls it automatically — you never call it yourself.