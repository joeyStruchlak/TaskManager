using Moq;
using FluentAssertions;
using TaskManager.Application.Commands.CreateProject;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tests.Commands;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _mockRepo;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _mockRepo = new Mock<IProjectRepository>();
        _handler = new CreateProjectCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsAddAsync()
    {
        // ARRANGE
        var command = new CreateProjectCommand("Test Project", "A description");

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsProjectId()
    {
        // ARRANGE
        var command = new CreateProjectCommand("Test Project", null);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesProjectWithCorrectName()
    {
        var command = new CreateProjectCommand("Test Project", "A description");
        Project? capturedProject = null;

        _mockRepo
            .Setup(r => r.AddAsync(It.IsAny<Project>()))
            .Callback<Project>(p => capturedProject = p);

        await _handler.Handle(command, CancellationToken.None);

        capturedProject!.Name.Should().Be("Test Project");
        capturedProject.Description.Should().Be("A description");
    }
}