// --- IMPORTS: Tools for building HTTP APIs and references to our Application Layer ---
using Microsoft.AspNetCore.Mvc;            // Gives us API tools ([ApiController], ControllerBase, Ok(), BadRequest())
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;         // Gives us CreateTaskRequest and TaskDto
using TaskManager.Application.UseCases;    // Gives us CreateTaskUseCase and GetAllTasksUseCase

namespace TaskManager.Api.Controllers;

/// <summary>
/// CONTROLLER: This is the front door of your web application.
/// It listents for web HTTP requests (GET, POST), hands the work to Use Cases,
/// and returns HTTP responses back to the browser or mobile app.
/// </summary>
[ApiController]                             // Tells ASP.NET: "This is a Web API controller with auto-validation"
[Route("api/[controller]")]                 // Defines the URL path: 'api/tasks' (replaces [controller] with 'Tasks')
public class TasksController : ControllerBase // Inherits built-in Web API helper functions from Microsoft
{
    // 1. Private variables to hold our Use Cases
    private readonly CreateTaskUseCase _createTaskUseCase;
    private readonly GetAllTasksUseCase _getAllTasksUseCase;

    // 2. CONSTRUCTOR: Asks .NET to inject our two Use Cases when a request comes in
    public TasksController(
        CreateTaskUseCase createTaskUseCase,
        GetAllTasksUseCase getAllTasksUseCase)
    {
        _createTaskUseCase = createTaskUseCase;   // Store the create use case
        _getAllTasksUseCase = getAllTasksUseCase; // Store the get all use case
    }

    /// <summary>
    /// GET ENDPOINT: Triggered when someone visits HTTP GET /api/tasks
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        // Ask the Use Case to do all the heavy lifting and fetch the tasks
        var tasks = await _getAllTasksUseCase.ExecuteAsync();

        // Return HTTP 200 OK along with the list of task DTOs
        return Ok(tasks);
    }

    /// <summary>
    /// POST ENDPOINT: Triggered when someone submits a new task via HTTP POST /api/tasks
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskRequest request)
    {
        // HTTP CONCERN: Basic check to ensure the user actually provided a title
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            // Return HTTP 400 Bad Request if the title is blank
            return BadRequest("Title is required.");
        }

        // Delegate the actual task creation to our Application Use Case
        var task = await _createTaskUseCase.ExecuteAsync(request);

        // Return HTTP 201 Created status code along with the newly created task DTO
        return StatusCode(201, task);
    }
}