using MediatR;                        // gives us IRequestHandler
using TaskManager.Application.DTOs;   // gives us TaskDto
using TaskManager.Domain.Interfaces;  // gives us ITaskRepository

namespace TaskManager.Application.Queries.GetAllTasks;

// This class HANDLES the GetAllTasksQuery
// IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>> means:
// "I handle GetAllTasksQuery and I return a list of TaskDtos"
public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
{
    // permanent slot to hold the database contract
    private readonly ITaskRepository _taskRepository;

    // constructor — ASP.NET automatically hands us a repository when this class is created
    public GetAllTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository; // save it permanently
    }

    // MediatR calls this when a GetAllTasksQuery comes in
    // "request" is the query — but it has no data on it, we just need the signal to go fetch
    public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        // go to the database and get all tasks — wait for it to finish
        var tasks = await _taskRepository.GetAllAsync();

        // loop through every task and convert each one from a TaskItem (domain) to a TaskDto (output)
        // we never expose raw domain entities to the outside world — always map to a DTO first
        return tasks.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt,
            Priority = task.Priority,
            AssignedTo = task.AssignedTo,
            DueDate = task.DueDate
        });
    }
}