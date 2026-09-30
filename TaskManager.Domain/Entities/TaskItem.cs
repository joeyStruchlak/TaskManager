namespace TaskManager.Domain.Entities;

public class TaskItem
{
    // private set = anyone can READ this, but only THIS class can WRITE it
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // constructor — the ONLY way to create a TaskItem
    // forces caller to always provide a Title
    // sets IsCompleted and CreatedAt automatically — can't be forgotten
    public TaskItem(string title, string? description = null)
    {
        Title = title;
        Description = description ?? string.Empty;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    // EF Core needs this to reconstruct objects from the database
    // private = your code can't accidentally use it
    private TaskItem() { }

    public void MarkComplete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Task is already completed.");

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new InvalidOperationException("Task is not completed.");

        IsCompleted = false;
        CompletedAt = null;
    }
}