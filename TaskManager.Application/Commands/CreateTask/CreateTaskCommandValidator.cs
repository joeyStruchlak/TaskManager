using FluentValidation;

namespace TaskManager.Application.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().WithMessage("Title is required.")
            .NotEmpty().WithMessage("Title is required.")
            .Must(t => !string.IsNullOrWhiteSpace(t)).WithMessage("Title cannot be empty or whitespace.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required.")
            .Must(p => new[] { "Low", "Medium", "High", "Critical" }.Contains(p))
            .WithMessage("Priority must be Low, Medium, High, or Critical.");

        RuleFor(x => x.AssignedTo)
            .EmailAddress().WithMessage("AssignedTo must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.AssignedTo));

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.")
            .When(x => x.DueDate.HasValue);
    }
}