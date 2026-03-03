using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.Commands.Validators;
using Xunit;

namespace HerdService.Tests.Commands.Validators;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandValidatorTests()
    {
        _validator = new CreateCategoryCommandValidator();
    }

    [Fact]
    public void CreateCategoryCommand_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var command = new CreateCategoryCommand("Dairy");

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
    public void CreateCategoryCommand_WithInvalidName_ShouldFailValidation(string name)
    {
        // Arrange
        var command = new CreateCategoryCommand(name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Category name is required");
    }

    [Fact]
    public void CreateCategoryCommand_WithTooLongName_ShouldFailValidation()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreateCategoryCommand(longName);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Category name cannot exceed 100 characters");
    }
}
