// --- NAMESPACE: The folder address in our project where this DTO lives ---
using TaskManager.Application.UseCases;

namespace TaskManager.Application.DTOs;

/// <summary>
/// INPUT DTO (Data Transfer Object)
/// This class represents the exact form data sent in by the user when creating a task.
/// </summary>
public class CreateTaskRequest
{
    // 1. Title typed in by the user on the screen or mobile app.
    // Defaults to empty string ("") so C# doesn't crash with null reference errors.
    public string Title { get; set; } = string.Empty;

    // 2. Description or extra details typed in by the user.
    // Defaults to empty string ("").
    public string Description { get; set; } = string.Empty;
}