namespace TaskManager.Domain.Entities;

public class TaskItem
{
    // private set = anyone can READ this, but only THIS class can WRITE it
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public string Priority { get; private set; } = string.Empty;   // "Low", "Medium", "High", "Critical"
    public string? AssignedTo { get; private set; } // staff member's email (M365)
    public DateTime? DueDate { get; private set; }  // when it's due
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }


    // constructor — the ONLY way to create a TaskItem
    // forces caller to always provide a Title
    // sets IsCompleted and CreatedAt automatically — can't be forgotten
    public TaskItem(
        string title,
        string? description = null,
        string priority = "Medium",
        string? assignedTo = null,
        DateTime? dueDate = null)
    {
        Title = title;
        Description = description ?? string.Empty;
        Priority = priority;
        AssignedTo = assignedTo;
        DueDate = dueDate;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
    string title,
    string? description,
    string priority,
    string? assignedTo,
    DateTime? dueDate)
    {
        Title = title;
        Description = description ?? string.Empty;
        Priority = priority;
        AssignedTo = assignedTo;
        DueDate = dueDate;
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