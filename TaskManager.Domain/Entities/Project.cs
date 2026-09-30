namespace TaskManager.Domain.Entities;

public class Project
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Project() { }

    public Project(string name, string? description)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}