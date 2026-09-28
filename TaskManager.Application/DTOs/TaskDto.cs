// --- NAMESPACE: Folder location for our DTOs ---
namespace TaskManager.Application.DTOs;

/// <summary>
/// OUTPUT DTO (Data Transfer Object)
/// This is the cleaned-up summary of a Task that we send BACK to the user's screen or app.
/// It contains all fields so the front-end knows everything about the task.
/// </summary>
public class TaskDto
{
    // 1. The unique ID assigned to this task by the database (e.g., 1, 2, 3...)
    public int Id { get; set; }

    // 2. The title of the task to display on screen.
    public string Title { get; set; } = string.Empty;

    // 3. The description of the task to display on screen.
    public string Description { get; set; } = string.Empty;

    // 4. True if completed (checkbox checked), false if still open.
    public bool IsCompleted { get; set; }

    // 5. The date and time this task was originally saved.
    public DateTime CreatedAt { get; set; }

    // 6. The date and time it was finished. Can be NULL (empty) if it's still open.
    public DateTime? CompletedAt { get; set; }
}