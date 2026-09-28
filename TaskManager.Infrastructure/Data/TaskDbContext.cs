// --- IMPORTS: Bringing in EF Core and our Domain Entity ---
using Microsoft.EntityFrameworkCore; // Brings in Entity Framework Core (our tool for talking to SQL)
using TaskManager.Domain.Entities;   // Brings in TaskItem

namespace TaskManager.Infrastructure.Data;

/// <summary>
/// DbContext acts as the bridge between your C# code and the SQL Database.
/// It represents a single session with the database.
/// </summary>
public class TaskDbContext : DbContext
{
    // 1. DbSet = A SQL Table. 
    // This tells EF Core: "Create a SQL table called 'Tasks' based on the 'TaskItem' C# class."
    public DbSet<TaskItem> Tasks { get; set; }

    // 2. CONSTRUCTOR: Takes database settings (like the connection string) 
    // and passes them up to the base DbContext class.
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    // 3. ON MODEL CREATING: Fine-tunes the database schema rules (column sizes, required fields).
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define specific SQL column rules for the TaskItem table
        modelBuilder.Entity<TaskItem>(entity =>
        {
            // Set 'Id' as the Primary Key in SQL
            entity.HasKey(e => e.Id);

            // Title is REQUIRED (NOT NULL) and cannot exceed 200 characters in SQL
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

            // Description is optional, but capped at 1000 characters in SQL
            entity.Property(e => e.Description).HasMaxLength(1000);

            // CreatedAt is REQUIRED (NOT NULL)
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}