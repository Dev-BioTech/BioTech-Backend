using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HealthService.Application.Commands;
using HealthService.Application.DTOs;
using HealthService.Presentation.Controllers;
using MediatR;

namespace HealthService.Tests.Controllers;

public class HealthEventControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<HealthService.Presentation.Services.GatewayAuthenticationService> _authServiceMock;
    private readonly HealthEventController _controller;

    public HealthEventControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _authServiceMock = new Mock<HealthService.Presentation.Services.GatewayAuthenticationService>();
        _controller = new HealthEventController(_mediatorMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task UpdateHealthEvent_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var eventId = 1L;
        var updateDto = new UpdateHealthEventDto(
            "Updated description",
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            "Updated treatment",
            "Dr. Smith"
        );
        var expectedEvent = new HealthEventResponse(
            eventId,
            1,
            1L,
            null,
            "Treatment",
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            null,
            "Updated treatment",
            null,
            null,
            null,
            "Dr. Smith",
            null,
            "Updated description",
            null,
            false,
            null,
            DateTime.UtcNow.AddDays(-2),
            DateTime.UtcNow
        );

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateHealthEventCommand>(), default))
                    .ReturnsAsync(expectedEvent);

        // Act
        var result = await _controller.UpdateHealthEvent(eventId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task UpdateHealthEvent_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var eventId = 1L;
        var updateDto = new UpdateHealthEventDto(
            "",
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Future date
            "",
            ""
        );

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateHealthEventCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Event date cannot be in the future"));

        // Act
        var result = await _controller.UpdateHealthEvent(eventId, updateDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetUpcomingHealthEvents_WithDefaultLimit_ShouldReturnOkResult()
    {
        // Arrange
        var expectedEvents = new List<UpcomingHealthEventDto>
        {
            new UpcomingHealthEventDto(1, "Animal-001", "Vaccination", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))),
            new UpcomingHealthEventDto(2, "Animal-002", "Check-up", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)))
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUpcomingHealthEventsQuery>(), default))
                    .ReturnsAsync(expectedEvents);

        // Act
        var result = await _controller.GetUpcomingHealthEvents();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetUpcomingHealthEvents_WithCustomLimit_ShouldReturnOkResult()
    {
        // Arrange
        var limit = 8;
        var expectedEvents = new List<UpcomingHealthEventDto>
        {
            new UpcomingHealthEventDto(1, "Animal-001", "Vaccination", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUpcomingHealthEventsQuery>(), default))
                    .ReturnsAsync(expectedEvents);

        // Act
        var result = await _controller.GetUpcomingHealthEvents(limit);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetUpcomingHealthEvents_WhenUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUpcomingHealthEventsQuery>(), default))
                    .ThrowsAsync(new UnauthorizedAccessException());

        // Act
        var result = await _controller.GetUpcomingHealthEvents();

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult.StatusCode.Should().Be(401);
    }
}
