using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.Commands.Validators;
using Xunit;

namespace HerdService.Tests.Commands.Validators;

public class CreateBreedCommandValidatorTests
{
    private readonly CreateBreedCommandValidator _validator;

    public CreateBreedCommandValidatorTests()
    {
        _validator = new CreateBreedCommandValidator();
    }

    [Fact]
    public void CreateBreedCommand_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var command = new CreateBreedCommand("Holstein");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateBreedCommand_WithInvalidName_ShouldFailValidation(string name)
    {
        // Arrange
        var command = new CreateBreedCommand(name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Breed name is required");
    }

    [Fact]
    public void CreateBreedCommand_WithTooLongName_ShouldFailValidation()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreateBreedCommand(longName);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Breed name cannot exceed 100 characters");
    }
}
