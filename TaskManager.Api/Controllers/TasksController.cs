using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Commands.CreateTask;
using TaskManager.Application.DTOs;
using TaskManager.Application.Queries.GetAllTasks;
using TaskManager.Application.Commands.CompleteTask;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // Controller now only knows about ONE thing — the mediator
    private readonly IMediator _mediator;

    // Constructor only needs one dependency now instead of two Use Cases
    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        // Create the query and hand it to MediatR — it finds the Handler automatically
        var tasks = await _mediator.Send(new GetAllTasksQuery());
        return Ok(tasks);
    }

    // POST /api/tasks?
    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskCommand command)
    {
        var task = await _mediator.Send(command);
        return StatusCode(201, task);
    }

    [HttpPatch("{id}/complete")]
        public async Task<ActionResult<bool>> Complete(int id)
    {
        // Create the command with the task ID and hand it to MediatR
        var result = await _mediator.Send(new CompleteTaskCommand(id));
        return Ok(result);
    }
}