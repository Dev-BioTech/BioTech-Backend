using Xunit;
using FluentAssertions;
using HerdService.Application.Commands;
using HerdService.Application.DTOs;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Enums;

namespace BioTechBackend.Tests.Integration;

/// <summary>
/// Tests de integración simplificados para verificar la funcionalidad básica
/// </summary>
public class BasicIntegrationTests
{
    [Fact]
    public void CreateBreedCommand_Should_Be_Valid()
    {
        // Arrange
        var command = new CreateBreedCommand("Test Breed");
        
        // Act & Assert
        command.Should().NotBeNull();
        command.Name.Should().Be("Test Breed");
    }

    [Fact]
    public void BreedResponse_Should_Be_Creatable()
    {
        // Arrange
        var response = new BreedResponse(1, "Test Breed");
        
        // Act & Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(1);
        response.Name.Should().Be("Test Breed");
    }

    [Fact]
    public void ProductDto_Should_Be_Creatable()
    {
        // Arrange
        var product = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Category = (int)ProductCategory.FEED,
            UnitOfMeasure = "kg",
            CurrentQuantity = 100,
            AverageCost = 50.5m,
            MinimumStock = 20,
            FarmId = 1,
            Active = true
        };
        
        // Act & Assert
        product.Should().NotBeNull();
        product.Name.Should().Be("Test Product");
        product.Category.Should().Be((int)ProductCategory.FEED);
    }

    [Fact]
    public void LowStockProductDto_Should_Be_Creatable()
    {
        // Arrange
        var product = new LowStockProductDto(1, "Low Stock Product", 5, 10);
        
        // Act & Assert
        product.Should().NotBeNull();
        product.Id.Should().Be(1);
        product.Name.Should().Be("Low Stock Product");
        product.CurrentQuantity.Should().Be(5);
        product.MinimumStock.Should().Be(10);
    }

    [Fact]
    public void Application_Layer_Should_Load_Correctly()
    {
        // Arrange & Act
        var herdAssembly = typeof(CreateBreedCommand).Assembly;
        var inventoryAssembly = typeof(ProductDto).Assembly;
        
        // Assert
        herdAssembly.Should().NotBeNull();
        inventoryAssembly.Should().NotBeNull();
        
        herdAssembly.GetTypes().Should().NotBeEmpty();
        inventoryAssembly.GetTypes().Should().NotBeEmpty();
    }
}
