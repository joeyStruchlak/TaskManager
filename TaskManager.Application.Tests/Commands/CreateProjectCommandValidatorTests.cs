using FluentAssertions;
using FluentValidation;
using TaskManager.Application.Commands.CreateProject;

namespace TaskManager.Application.Tests.Commands;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact] // this is a test method attribute from xUnit
    public void Validate_EmptyName_ShouldFail()
    {
        var command = new CreateProjectCommand("", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Project name is required.");
    }

    [Fact]
    public void Validate_ValidName_ShouldPass()
    {
        var command = new CreateProjectCommand("My Project", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_NullName_ShouldFail()
    {
        var command = new CreateProjectCommand(null!, null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Project name is required.");
    }

    [Fact]
    public void Validate_WhitespaceName_ShouldFail()
    {
        var command = new CreateProjectCommand("   ", null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Project name is required.");
    }

    [Fact]
    public void Validate_NameExceeds200Chars_ShouldFail()
    {
        var command = new CreateProjectCommand(new string('A', 201), null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Project name cannot exceed 200 characters.");
    }

    [Fact]
    public void Validate_NameExactly200Chars_ShouldPass()
    {
        var command = new CreateProjectCommand(new string('A', 200), null);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}