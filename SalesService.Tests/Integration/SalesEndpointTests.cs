using System.Net.Http.Json;
using Xunit;
using System.Net;
using FluentAssertions;

namespace SalesService.Tests.Integration;

public class SalesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public SalesEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock sales repository
                var saleRepositoryMock = new Mock<ISaleRepository>();
                saleRepositoryMock.Setup(x => x.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Sale>
                    {
                        new Sale(1, 1L, "John Doe", DateTime.UtcNow.AddDays(-5), 1500.00m),
                        new Sale(1, 2L, "Jane Smith", DateTime.UtcNow.AddDays(-3), 1200.00m)
                    });
                
                services.AddScoped<ISaleRepository>(_ => saleRepositoryMock.Object);
            });
        });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetSales_ShouldReturnUserSales()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.GetAsync("/api/sales");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var sales = await response.Content.ReadFromJsonAsync<List<SaleDto>>();
        sales.Should().NotBeNull();
        sales.Should().HaveCount(2);
        sales.Should().OnlyContain(s => s.Amount > 0);
    }

    [Fact]
    public async Task CreateSale_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var saleData = new CreateSaleDto(1, 3L, "Bob Wilson", DateTime.UtcNow, 1800.00m, "Excellent condition");

        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", saleData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdSale = await response.Content.ReadFromJsonAsync<SaleDto>();
        createdSale.Should().NotBeNull();
        createdSale.BuyerName.Should().Be("Bob Wilson");
        createdSale.Amount.Should().Be(1800.00m);
    }

    [Fact]
    public async Task UpdateSale_WithValidData_ShouldReturnOk()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");
        
        var updateDto = new UpdateSaleDto("Updated Buyer", DateTime.UtcNow.AddDays(-1), 2000.00m);

        // Act
        var response = await _client.PutAsJsonAsync("/api/sales/1", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedSale = await response.Content.ReadFromJsonAsync<SaleDto>();
        updatedSale.Should().NotBeNull();
        updatedSale.BuyerName.Should().Be("Updated Buyer");
        updatedSale.Amount.Should().Be(2000.00m);
    }

    [Fact]
    public async Task DeleteSale_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "valid-jwt-token");

        // Act
        var response = await _client.DeleteAsync("/api/sales/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetSales_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange - No auth header

        // Act
        var response = await _client.GetAsync("/api/sales");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
