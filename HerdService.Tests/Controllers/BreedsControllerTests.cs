using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using HerdService.Application.Commands;
using HerdService.Application.DTOs;
using HerdService.Application.Queries;
using HerdService.Presentation.Controllers.V1;
using HerdService.Presentation.Services;
using MediatR;

namespace HerdService.Tests.Controllers;

public class BreedsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<GatewayAuthenticationService> _authServiceMock;
    private readonly BreedsController _controller;

    public BreedsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _authServiceMock = new Mock<GatewayAuthenticationService>();
        _controller = new BreedsController(_mediatorMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task GetAllBreeds_WhenSuccessful_ShouldReturnOkResult()
    {
        // Arrange
        var expectedBreeds = new List<BreedResponse>
        {
            new BreedResponse(1, "Holstein"),
            new BreedResponse(2, "Jersey")
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllBreedsQuery>(), default))
                    .ReturnsAsync(expectedBreeds);

        // Act
        var result = await _controller.GetAllBreeds();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        
        // The actual response structure depends on your ApiResponse wrapper
        // This is a basic test - adjust according to your actual response structure
    }

    [Fact]
    public async Task CreateBreed_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new CreateBreedCommand("Holstein");
        var expectedBreed = new BreedResponse(1, "Holstein");

        _mediatorMock.Setup(x => x.Send(command, default))
                    .ReturnsAsync(expectedBreed);

        // Act
        var result = await _controller.CreateBreed(command);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task CreateBreed_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new CreateBreedCommand("");
        _mediatorMock.Setup(x => x.Send(command, default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Name is required"));

        // Act
        var result = await _controller.CreateBreed(command);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
