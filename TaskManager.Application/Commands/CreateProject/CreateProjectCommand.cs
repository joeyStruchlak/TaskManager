using MediatR;

namespace TaskManager.Application.Commands.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : IRequest<int>;