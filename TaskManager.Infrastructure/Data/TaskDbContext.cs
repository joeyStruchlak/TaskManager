using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data;

public class TaskDbContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Timesheet> Timesheets { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubmittedBy).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SupervisorEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TotalHours).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SubmittedAt).IsRequired();
        });


    }
}