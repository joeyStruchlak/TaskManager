namespace TaskManager.Application.DTOs;

/// <summary>
/// INPUT DTO (Data Transfer Object)
/// This class represents the exact form data sent in by the user when creating a task.
/// </summary>
public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}