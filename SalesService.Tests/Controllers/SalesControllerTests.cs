using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SalesService.Application.Commands;
using SalesService.Application.DTOs;
using SalesService.Presentation.Controllers;
using MediatR;

namespace SalesService.Tests.Controllers;

public class SalesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new SalesController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetSales_WhenSuccessful_ShouldReturnOkResult()
    {
        // Arrange
        var expectedSales = new List<SaleDto>
        {
            new SaleDto(1, 1, 1L, "John Doe", DateTime.UtcNow.AddDays(-5), 1500.00m, null, DateTime.UtcNow.AddDays(-5)),
            new SaleDto(2, 1, 2L, "Jane Smith", DateTime.UtcNow.AddDays(-3), 1200.00m, null, DateTime.UtcNow.AddDays(-3))
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetSalesByUserQuery>(), default))
                    .ReturnsAsync(expectedSales);

        // Act
        var result = await _controller.GetSales();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetSales_WhenUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetSalesByUserQuery>(), default))
                    .ThrowsAsync(new UnauthorizedAccessException());

        // Act
        var result = await _controller.GetSales();

        // Assert
        result.Result.Should().BeOfType<UnauthorizedResult>();
        var unauthorizedResult = result.Result as UnauthorizedResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task CreateSale_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var createDto = new CreateSaleDto(1, 1L, "John Doe", DateTime.UtcNow, 1500.00m, "Good condition");
        var expectedSale = new SaleDto(1, 1, 1L, "John Doe", DateTime.UtcNow, 1500.00m, "Good condition", DateTime.UtcNow);

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateSaleCommand>(), default))
                    .ReturnsAsync(expectedSale);

        // Act
        var result = await _controller.CreateSale(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task CreateSale_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var createDto = new CreateSaleDto(1, 1L, "", DateTime.UtcNow.AddDays(1), -100.00m);
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateSaleCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

        // Act
        var result = await _controller.CreateSale(createDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateSale_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var saleId = 1;
        var updateDto = new UpdateSaleDto("Updated Buyer", DateTime.UtcNow.AddDays(-1), 1800.00m);
        var expectedSale = new SaleDto(1, 1, 1L, "Updated Buyer", DateTime.UtcNow.AddDays(-1), 1800.00m, null, DateTime.UtcNow.AddDays(-2));

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateSaleCommand>(), default))
                    .ReturnsAsync(expectedSale);

        // Act
        var result = await _controller.UpdateSale(saleId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task UpdateSale_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var saleId = 1;
        var updateDto = new UpdateSaleDto("", DateTime.UtcNow.AddDays(1), -100.00m);
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateSaleCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

        // Act
        var result = await _controller.UpdateSale(saleId, updateDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteSale_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var saleId = 1;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteSaleCommand>(), default))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSale(saleId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteSale_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var saleId = 999;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteSaleCommand>(), default))
                    .ThrowsAsync(new ArgumentException("Sale not found"));

        // Act
        var result = await _controller.DeleteSale(saleId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);
    }
}
