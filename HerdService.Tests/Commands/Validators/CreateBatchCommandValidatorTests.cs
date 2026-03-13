using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.Commands.Validators;
using Xunit;

namespace HerdService.Tests.Commands.Validators;

public class CreateBatchCommandValidatorTests
{
    private readonly CreateBatchCommandValidator _validator;

    public CreateBatchCommandValidatorTests()
    {
        _validator = new CreateBatchCommandValidator();
    }

    [Fact]
    public void CreateBatchCommand_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var command = new CreateBatchCommand("Batch A", 1);

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
    public void CreateBatchCommand_WithInvalidName_ShouldFailValidation(string name)
    {
        // Arrange
        var command = new CreateBatchCommand(name, 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Batch name is required");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateBatchCommand_WithInvalidFarmId_ShouldFailValidation(int farmId)
    {
        // Arrange
        var command = new CreateBatchCommand("Batch A", farmId);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Valid FarmId is required");
    }

    [Fact]
    public void CreateBatchCommand_WithTooLongName_ShouldFailValidation()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreateBatchCommand(longName, 1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Batch name cannot exceed 100 characters");
    }
}
