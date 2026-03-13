using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.Commands.Validators;
using Xunit;

namespace HerdService.Tests.Commands.Validators;

public class CreatePaddockCommandValidatorTests
{
    private readonly CreatePaddockCommandValidator _validator;

    public CreatePaddockCommandValidatorTests()
    {
        _validator = new CreatePaddockCommandValidator();
    }

    [Fact]
    public void CreatePaddockCommand_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var command = new CreatePaddockCommand("North Field", 1);

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
    public void CreatePaddockCommand_WithInvalidName_ShouldFailValidation(string name)
    {
        // Arrange
        var command = new CreatePaddockCommand(name, 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Paddock name is required");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreatePaddockCommand_WithInvalidFarmId_ShouldFailValidation(int farmId)
    {
        // Arrange
        var command = new CreatePaddockCommand("North Field", farmId);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Valid FarmId is required");
    }

    [Fact]
    public void CreatePaddockCommand_WithTooLongName_ShouldFailValidation()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreatePaddockCommand(longName, 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Paddock name cannot exceed 100 characters");
    }
}
