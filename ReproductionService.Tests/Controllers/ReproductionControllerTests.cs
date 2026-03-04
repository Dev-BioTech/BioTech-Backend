using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ReproductionService.Application.Commands;
using ReproductionService.Application.DTOs;
using ReproductionService.Presentation.Controllers;
using MediatR;

namespace ReproductionService.Tests.Controllers;

public class ReproductionControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ReproductionService.Presentation.Services.GatewayAuthenticationService> _authServiceMock;
    private readonly ReproductionController _controller;

    public ReproductionControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _authServiceMock = new Mock<ReproductionService.Presentation.Services.GatewayAuthenticationService>();
        _controller = new ReproductionController(_mediatorMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task GetPregnanciesByFarm_WithValidFarmId_ShouldReturnOkResult()
    {
        // Arrange
        var farmId = 1;
        var expectedPregnancies = new List<PregnancyDto>
        {
            new PregnancyDto(1L, "Animal-001", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(100)), 20),
            new PregnancyDto(2L, "Animal-002", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(90)), 18)
        };

        _authServiceMock.Setup(x => x.GetUserId()).Returns(123);
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetPregnanciesByFarmQuery>(), default))
                    .ReturnsAsync(expectedPregnancies);

        // Act
        var result = await _controller.GetPregnanciesByFarm(farmId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetPregnanciesByFarm_WhenUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var farmId = 1;
        _authServiceMock.Setup(x => x.GetUserId()).Returns((int?)null);

        // Act
        var result = await _controller.GetPregnanciesByFarm(farmId);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task GetBirthsByFarm_WithValidFarmId_ShouldReturnOkResult()
    {
        // Arrange
        var farmId = 1;
        var expectedBirths = new List<BirthDto>
        {
            new BirthDto(1L, "Calf-001", DateTime.UtcNow.AddDays(-5), 35.5m, "Male"),
            new BirthDto(2L, "Calf-002", DateTime.UtcNow.AddDays(-3), 32.0m, "Female")
        };

        _authServiceMock.Setup(x => x.GetUserId()).Returns(123);
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetBirthsByFarmQuery>(), default))
                    .ReturnsAsync(expectedBirths);

        // Act
        var result = await _controller.GetBirthsByFarm(farmId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task RegisterBirth_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new RegisterBirthCommand(1L, "Calf-001", 35.5m, "Male", DateTime.UtcNow.AddDays(-1));
        var expectedBirth = new BirthDto(1L, "Calf-001", DateTime.UtcNow.AddDays(-1), 35.5m, "Male");

        _authServiceMock.Setup(x => x.GetUserId()).Returns(123);
        _mediatorMock.Setup(x => x.Send(command, default))
                    .ReturnsAsync(expectedBirth);

        // Act
        var result = await _controller.RegisterBirth(command);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task RegisterBirth_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new RegisterBirthCommand(1L, "", -5.0m, "Invalid", DateTime.UtcNow.AddDays(1));
        _authServiceMock.Setup(x => x.GetUserId()).Returns(123);
        _mediatorMock.Setup(x => x.Send(command, default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

        // Act
        var result = await _controller.RegisterBirth(command);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task RegisterBirth_WhenUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var command = new RegisterBirthCommand(1L, "Calf-001", 35.5m, "Male", DateTime.UtcNow.AddDays(-1));
        _authServiceMock.Setup(x => x.GetUserId()).Returns((int?)null);

        // Act
        var result = await _controller.RegisterBirth(command);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult.StatusCode.Should().Be(401);
    }
}
