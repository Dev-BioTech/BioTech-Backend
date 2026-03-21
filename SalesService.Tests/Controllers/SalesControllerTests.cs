using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SalesService.Application.Commands;
using SalesService.Application.DTOs;
using SalesService.Presentation.Controllers.V1;
using SalesService.Application.Queries;
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
    public async Task GetSales_WhenUnauthorized_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetSalesByUserQuery>(), default))
                    .ThrowsAsync(new UnauthorizedAccessException());

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.GetSales());
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
    public async Task CreateSale_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateSaleDto(1, 1L, "", DateTime.UtcNow.AddDays(1), -100.00m);
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateSaleCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _controller.CreateSale(createDto));
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
    public async Task UpdateSale_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var saleId = 1;
        var updateDto = new UpdateSaleDto("", DateTime.UtcNow.AddDays(1), -100.00m);
        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateSaleCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _controller.UpdateSale(saleId, updateDto));
    }

    [Fact]
    public async Task DeleteSale_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var saleId = 1;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteSaleCommand>(), default))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSale(saleId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteSale_WithInvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        var saleId = 999;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteSaleCommand>(), default))
                    .ThrowsAsync(new ArgumentException("Sale not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _controller.DeleteSale(saleId));
    }
}
