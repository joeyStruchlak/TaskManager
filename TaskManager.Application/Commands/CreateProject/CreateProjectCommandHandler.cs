using MediatR;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Commands.CreateProject;

// IRequestHandler<TRequest, TResponse>
// TRequest = CreateProjectCommand (what comes in)
// TResponse = int (the new project's Id going back out)
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    // FIELD: our repository — the only way this handler touches the database
    private readonly IProjectRepository _projectRepository;

    // CONSTRUCTOR: DI injects IProjectRepository automatically
    // Notice we depend on the INTERFACE not the concrete class — Clean Architecture rule
    public CreateProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    // HANDLE: MediatR calls this automatically when controller sends CreateProjectCommand
    // 'command' = the data that came in (Name, Description)
    // 'cancellationToken' = lets the request cancel if the user disconnects — always include it
    public async Task<int> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        // Create the domain entity — Project constructor enforces business rules
        var project = new Project(command.Name, command.Description);

        // Persist to database via repository
        await _projectRepository.AddAsync(project);

        // Return the new Id so the API can send it back to the caller
        return project.Id;
    }
}