using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Presentation.Controllers;
using MediatR;

namespace InventoryService.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var productId = 1;
        var updateDto = new UpdateProductDto 
        { 
            Name = "Updated Product",
            UnitOfMeasure = "kg",
            MinimumStock = 10
        };
        var expectedProduct = new ProductDto
        {
            Id = productId,
            Name = "Updated Product",
            UnitOfMeasure = "kg",
            MinimumStock = 10,
            FarmId = 1,
            CurrentQuantity = 50,
            AverageCost = 5.0m,
            Active = true
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), default))
                    .ReturnsAsync(expectedProduct);

        // Act
        var result = await _controller.UpdateProduct(productId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var productId = 1;
        var updateDto = new UpdateProductDto 
        { 
            Name = "",
            UnitOfMeasure = "kg",
            MinimumStock = 10
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), default))
                    .ThrowsAsync(new FluentValidation.ValidationException("Name is required"));

        // Act
        var result = await _controller.UpdateProduct(productId, updateDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var productId = 1;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), default))
                    .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var productId = 999;

        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteProductCommand>(), default))
                    .ThrowsAsync(new ArgumentException("Product not found"));

        // Act
        var result = await _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetLowStockProducts_WithValidFarmId_ShouldReturnOkResult()
    {
        // Arrange
        var farmId = 1;
        var expectedProducts = new List<LowStockProductDto>
        {
            new LowStockProductDto(1, "Product 1", 5, 10),
            new LowStockProductDto(2, "Product 2", 8, 15)
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetLowStockProductsQuery>(), default))
                    .ReturnsAsync(expectedProducts);

        // Act
        var result = await _controller.GetLowStockProducts(farmId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
    }
}
