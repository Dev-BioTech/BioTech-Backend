using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;

namespace InventoryService.Tests.Integration;

public class ProductsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ProductsEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock product repository
                var productRepositoryMock = new Mock<IProductRepository>();
                productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Product
                    {
                        Id = 1,
                        Name = "Test Product",
                        FarmId = 1,
                        CurrentQuantity = 100,
                        MinimumStock = 10,
                        UnitOfMeasure = "kg",
                        Active = true
                    });
                
                services.AddScoped<IProductRepository>(_ => productRepositoryMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ShouldReturnOk()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var updateDto = new UpdateProductDto 
        { 
            Name = "Updated Product",
            UnitOfMeasure = "kg",
            MinimumStock = 15
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/Products/1", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        updatedProduct.Should().NotBeNull();
        updatedProduct.Name.Should().Be("Updated Product");
        updatedProduct.MinimumStock.Should().Be(15);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.DeleteAsync("/api/Products/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetLowStockProducts_ShouldReturnLowStockItems()
    {
        // Arrange
        _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var productRepositoryMock = new Mock<IProductRepository>();
                productRepositoryMock.Setup(x => x.GetLowStockProductsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Product>
                    {
                        new Product { Id = 1, Name = "Product 1", CurrentQuantity = 5, MinimumStock = 10 },
                        new Product { Id = 2, Name = "Product 2", CurrentQuantity = 8, MinimumStock = 15 }
                    });
                
                services.AddScoped<IProductRepository>(_ => productRepositoryMock.Object);
            });
        });

        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/Products/low-stock?farmId=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var lowStockProducts = await response.Content.ReadFromJsonAsync<List<LowStockProductDto>>();
        lowStockProducts.Should().NotBeNull();
        lowStockProducts.Should().HaveCount(2);
        lowStockProducts.Should().OnlyContain(p => p.CurrentQuantity <= p.MinimumStock);
    }
}
