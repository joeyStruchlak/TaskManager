using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskWorkflowService _workflowService;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        ITaskWorkflowService workflowService)
    {
        _taskRepository = taskRepository;
        _workflowService = workflowService;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(
            request.Title,
            request.Description,
            request.Priority,
            request.AssignedTo,
            request.DueDate);

        var createdTask = await _taskRepository.AddAsync(task);

        await _workflowService.StartAssignmentWorkflowAsync(createdTask.Id);

        return new TaskDto { /* same as before */ };
    }
}